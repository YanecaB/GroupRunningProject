using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Event;
using CinemaApp.Web.ViewModels.Group;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MockQueryable;
using Moq;
using static CinemaApp.Common.EntityValidationMessages;

namespace CinemaApp.Tests
{
    [TestFixture]
    public class GroupServiceTests
	{
        private IEnumerable<Group> groupsData = new List<Group>()
        {
            new Group()
            {
                Id = Guid.Parse("e3edd800-9da8-4955-89bd-25469b59ac3d"),
                Name = "Evening Joggers",
                Description = "For those who prefer running in the evening to unwind after work.",
                Location = "Sunset Boulevard, Los Angeles",
                CreatedDate = DateTime.Now,
                AdminId = Guid.Parse("7b242b58-cd6b-4fd9-a3de-29c795527093")
            },
            new Group()
            {
                Id = Guid.Parse("95865a98-7985-40cf-80c1-7ee4cb61f24e"),
                Name = "Weekend Warriors",
                Description = "A group for those who only run on weekends but still take it seriously.",
                Location = "Golden Gate Park, San Francisco",
                CreatedDate = DateTime.Now,
                AdminId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6")
            }
        };

        private IEnumerable<Membership> membershipsData = new List<Membership>()
        {
            new Membership() { GroupId = Guid.Parse("e3edd800-9da8-4955-89bd-25469b59ac3d"), ApplicationUserId = Guid.NewGuid() }
        };

        private IEnumerable<Event> eventsData = new List<Event>()
        {
            new Event()
            {
                Id = Guid.NewGuid(),
                Title = "Novogodishen jogging",
                Description = "We will jog at the start of 2025",
                Date = DateTime.Now,
                Location = "Plovdiv/Bulgaria",
                OrganizerId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                Distance = 20,
                GroupId = Guid.Parse("089d31fe-18ff-4f24-8d76-cbab61c7c61a")
            }
        };


        private Mock<IRepository<Event, Guid>> eventRepository;
        private Mock<IRepository<Group, Guid>> groupRepository;        
        private Mock<IRepository<Membership, Guid>> membershipRepository;

        [SetUp]
        public void Setup()
        {
            this.eventRepository = new Mock<IRepository<Event, Guid>>();
            this.groupRepository = new Mock<IRepository<Group, Guid>>();
            this.membershipRepository = new Mock<IRepository<Membership, Guid>>();           
        }

        [Test]
        public async Task GetAllGroupsNoFilterPositive()
        {
            IQueryable<Group> groupsMockQueryable = groupsData.BuildMock();

            this.groupRepository.Setup(r => r.GetAllAttached())
                .Returns(groupsMockQueryable);

            IGroupService groupService = new GroupService(groupRepository.Object, membershipRepository.Object, eventRepository.Object);
            IEnumerable<GroupIndexViewModel> allGroupsActual = await groupService.IndexGetAllAsync();

            Assert.IsNotNull(allGroupsActual);
            Assert.AreEqual(this.groupsData.Count(), allGroupsActual.Count());
        }

        [Test]
        [TestCase("We")]
        [TestCase("we")]
        public async Task GetAllGroupsSearchQueryPositive(string searchQuery)
        {
            int expectedGroupsCount = 1;
            string expectedGroupId = "95865a98-7985-40cf-80c1-7ee4cb61f24e";

            IQueryable<Group> groupsMockQueryable = groupsData.BuildMock();
            this.groupRepository.Setup(r => r.GetAllAttached())
                .Returns(groupsMockQueryable);

            IGroupService groupService = new GroupService(groupRepository.Object, membershipRepository.Object, eventRepository.Object);
            IEnumerable<GroupIndexViewModel> allGroupsActual = await groupService.IndexGetAllAsync(searchQuery);

            Assert.IsNotNull(allGroupsActual);
            Assert.AreEqual(expectedGroupsCount, allGroupsActual.Count());
            Assert.AreEqual(expectedGroupId.ToLower(), allGroupsActual.First().Id.ToLower());
        }

        [Test]
        [TestCase("shrek")]
        public async Task GetAllGroupsSearchQueryNoMatches(string searchQuery)
        {
            int expectedGroupsCount = 0;

            IQueryable<Group> groupsMockQueryable = groupsData.BuildMock();
            this.groupRepository.Setup(r => r.GetAllAttached())
                .Returns(groupsMockQueryable);

            IGroupService groupService = new GroupService(groupRepository.Object, membershipRepository.Object, eventRepository.Object);
            IEnumerable<GroupIndexViewModel> allGroupsActual = await groupService.IndexGetAllAsync(searchQuery);

            Assert.IsNotNull(allGroupsActual);
            Assert.AreEqual(expectedGroupsCount, allGroupsActual.Count());            
        }

        [Test]        
        public async Task AddGroupPositive()
        {
            GroupCreateViewModel viewModel = new GroupCreateViewModel()
            {
                Name = "Test Group",
                Description = "dddadasddqwdqeqw",
                Location = "fdwdqwidqdqojdqwnojdqwjondq",
                CreatedDate = DateTime.Now
            };

            Guid userGuid = Guid.NewGuid();

            IGroupService groupService = new GroupService(groupRepository.Object, membershipRepository.Object, eventRepository.Object);
            await groupService.AddGroupAsync(viewModel, userGuid);
            groupRepository.Verify(gr => gr.AddAsync(It.Is<Group>(g => g.Name == "Test Group" && g.AdminId == userGuid)), Times.Once);

            membershipRepository.Setup(mr => mr.FirstOrDefaultAsync(It.IsAny<Expression<Func<Membership, bool>>>()))
                .ReturnsAsync((Membership)null);
        }

        [Test]
        public async Task FollowGroup()
        {
            Guid groupId = Guid.Parse("e3edd800-9da8-4955-89bd-25469b59ac3d");
            Guid userGuid = Guid.NewGuid();
            
            var groupMock = new Mock<Group>();

            groupRepository.Setup(gr => gr.GetByIdAsync(groupId)).ReturnsAsync(groupMock.Object);

            membershipRepository.Setup(mr => mr.FirstOrDefaultAsync(It.IsAny<Expression<Func<Membership, bool>>>()))
                .ReturnsAsync((Membership)null);

            IGroupService groupService = new GroupService(groupRepository.Object, membershipRepository.Object, eventRepository.Object);
            
            await groupService.FollowGroupAsync(groupId, userGuid);            
            membershipRepository.Verify(mr => mr.AddAsync(It.Is<Membership>(m => m.ApplicationUserId == userGuid && m.GroupId == groupId)), Times.Once);
        }

        public async Task GetGroupDetailsById()
        {
            var groupId = groupsData.First().Id;
            var userGuidId = Guid.NewGuid();
            
            groupRepository.Setup(r => r.GetAllAttached())
                .Returns(groupsData.AsQueryable());
            
            membershipRepository.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Membership, bool>>>()))
                .ReturnsAsync(membershipsData.First());

            groupRepository.Setup(r => r.GetAllAttached())
                .Returns(groupsData.AsQueryable());
            
            var groupService = new GroupService(groupRepository.Object, membershipRepository.Object, eventRepository.Object);
            var result = await groupService.GetGroupDetailsByIdAsync(groupId, userGuidId);

            Assert.IsNotNull(result);
            Assert.AreEqual(groupId.ToString(), result.Id);
            Assert.AreEqual(groupsData.First().Name, result.Name);
            Assert.AreEqual(groupsData.First().Description, result.Description);
            Assert.AreEqual(groupsData.First().Location, result.Location);
            Assert.AreEqual(1, result.MembersCount);
            Assert.IsTrue(result.IsFollowing); 
        }

        [Test]
        public async Task GetGroupDetailsByIdReturnNullWhenGroupIsDeleted()
        {            
            var deletedGroupId = Guid.NewGuid();
            var userGuidId = Guid.NewGuid();

            var groupsData = new List<Group>()
            {
                new Group()
                {
                    Id = deletedGroupId,
                    Name = "Deleted Group",
                    Description = "This group is deleted",
                    Location = "Deleted Location",
                    CreatedDate = DateTime.Now,
                    AdminId = Guid.NewGuid(),
                    IsDeleted = true,
                    Memberships = new List<Membership>()
                }
            }.AsQueryable().BuildMock();

            groupRepository.Setup(r => r.GetAllAttached())
                .Returns(groupsData);

            var groupService = new GroupService(groupRepository.Object, membershipRepository.Object, eventRepository.Object);
            
            var result = await groupService.GetGroupDetailsByIdAsync(deletedGroupId, userGuidId);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetGroupDetailsByIdAShouldReturnCorrectDetailsWhenGroupIsNotFollowed()
        {            
            var groupId = groupsData.First().Id;
            var userGuidId = Guid.NewGuid();
            
            groupRepository.Setup(r => r.GetAllAttached())
                .Returns(groupsData.AsQueryable().BuildMock());
            
            membershipRepository.Setup(m => m.FirstOrDefaultAsync(It.IsAny<Expression<Func<Membership, bool>>>()))
                .ReturnsAsync((Membership)null);

            eventRepository.Setup(e => e.GetAllAttached())
                .Returns(eventsData.AsQueryable());
            
            var groupService = new GroupService(groupRepository.Object, membershipRepository.Object, eventRepository.Object);
            var result = await groupService.GetGroupDetailsByIdAsync(groupId, userGuidId);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsFollowing); 
        }

        [Test]
        public async Task RemoveFollowerPositive()
        {
            Guid groupId = this.membershipsData.First().GroupId;
            Guid followerId = this.membershipsData.First().ApplicationUserId;

            Membership membership = new Membership()
            {
                GroupId = groupId,
                ApplicationUserId = followerId
            };

            membershipRepository
                .Setup(mr => mr.FirstOrDefaultAsync(It.IsAny<Expression<Func<Membership, bool>>>()))
                .ReturnsAsync(membership);

            membershipRepository
                .Setup(mr => mr.DeleteAsync(It.IsAny<Membership>()))
                .ReturnsAsync(true);

            var groupService = new GroupService(groupRepository.Object, membershipRepository.Object, eventRepository.Object);

            bool test = await groupService.RemoveFollowerAsync(groupId, followerId);

            Assert.IsTrue(test);
            membershipRepository.Verify(mr => mr.FirstOrDefaultAsync(It.IsAny<Expression<Func<Membership, bool>>>()), Times.Once);
            membershipRepository.Verify(mr => mr.DeleteAsync(It.Is<Membership>(m => m.GroupId == groupId && m.ApplicationUserId == followerId)), Times.Once);
        }

        [Test]
        public async Task EditGroupPositive()
        {            
            var groupId = Guid.NewGuid();
            var originalGroup = new Group
            {
                Id = groupId,
                Name = "Original Name",
                Description = "Original Description",
                Location = "Original Location",
                CreatedDate = DateTime.Now
            };

            var viewModel = new GroupEditViewModel
            {
                Id = groupId.ToString(),
                Name = "Updated Name",
                Description = "Updated Description",
                Location = "Updated Location",
                CreatedDate = DateTime.Now.AddDays(-1)
            };

            groupRepository.Setup(gr => gr.GetByIdAsync(groupId)).ReturnsAsync(originalGroup);

            groupRepository.Setup(gr => gr.UpdateAsync(It.IsAny<Group>())).ReturnsAsync(true);

            var groupService = new GroupService(groupRepository.Object, membershipRepository.Object, eventRepository.Object);
            
            var result = await groupService.EditGroupAsync(viewModel);
            
            Assert.IsTrue(result, "EditGroupAsync should return true when update succeeds.");
            Assert.AreEqual(viewModel.Name, originalGroup.Name, "Group name should be updated.");
            Assert.AreEqual(viewModel.Description, originalGroup.Description, "Group description should be updated.");
            Assert.AreEqual(viewModel.Location, originalGroup.Location, "Group location should be updated.");
            Assert.AreEqual(viewModel.CreatedDate, originalGroup.CreatedDate, "Group created date should be updated.");
            
            groupRepository.Verify(gr => gr.UpdateAsync(It.Is<Group>(g =>
                g.Id == groupId &&
                g.Name == viewModel.Name &&
                g.Description == viewModel.Description &&
                g.Location == viewModel.Location &&
                g.CreatedDate == viewModel.CreatedDate
            )), Times.Once);
        }
    }
}


