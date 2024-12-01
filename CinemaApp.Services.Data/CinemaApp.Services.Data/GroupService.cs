using System;
using System.Security.Claims;
using CinemaApp.Common;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Group;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using CinemaApp.Web.ViewModels.Event;
using CinemaApp.Common;
using Microsoft.VisualBasic;

namespace CinemaApp.Services.Data
{
    public class GroupService : BaseService, IGroupService
    {
        private readonly IRepository<Group, Guid> groupRepository;
        private readonly IRepository<Event, Guid> eventRepository;
        private readonly IRepository<Membership, object> membershipRepository;

        public GroupService(IRepository<Group, Guid> groupRepository,
            IRepository<Membership, object> membershipRepository,
            IRepository<Event, Guid> eventRepository)
        {
            this.groupRepository = groupRepository;
            this.membershipRepository = membershipRepository;
            this.eventRepository = eventRepository;
        }

        public async Task<IEnumerable<GroupIndexViewModel>> IndexGetAllAsync()
        {
            IEnumerable<GroupIndexViewModel> groups = await this.groupRepository
                .GetAllAttached()
                .Where(g => g.IsDeleted == false)
                .Select(c => new GroupIndexViewModel()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location,
                    Description = c.Description,
                    CreatedDate = c.CreatedDate.ToString(EntityValidationConstants.Group.ReleaseDateFormat)
                })
                .OrderBy(c => c.Location)
                .ToArrayAsync();

            return groups;
        }        

        public async Task AddGroupAsync(GroupCreateViewModel viewModel, Guid adminId)
        {
            Group newGroup = new Group()
            {
                Id = Guid.NewGuid(),
                Name = viewModel.Name,
                Description = viewModel.Description,
                Location = viewModel.Location,
                CreatedDate = viewModel.CreatedDate,
                AdminId = adminId
            };
           
            await groupRepository.AddAsync(newGroup);

            await membershipRepository.AddAsync(new Membership()
            {
                ApplicationUserId = newGroup.AdminId,
                GroupId = newGroup.Id
            });
        }
        
        public async Task<GroupDetailsViewModel> GetGroupDetailsByIdAsync(Guid id, Guid userGuidId)
        {
            var groups = await this.groupRepository
                .GetAllAttached()
                .Include(g => g.Memberships)                
                .ToArrayAsync();

            var group = groups.FirstOrDefault(g => g.Id == id);

            GroupDetailsViewModel? viewModel = null;
                       
            if (group != null && group.IsDeleted == false)
            {
                var events = (await this.groupRepository.GetAllAttached().Include(g => g.Events).ToArrayAsync()).FirstOrDefault(g => g.Id == id).Events.Select(e => new EventIndexViewModel()
                {
                    Id = e.Id.ToString(),
                    Date = e.Date.ToString(EntityValidationConstants.Event.DateFormat),
                    Title = e.Title,
                    GroupName = group.Name,
                    AdminId = e.OrganizerId.ToString()
                }).ToList();

                int membersCount = group.Memberships.Count();

                var isFollowing = await this.membershipRepository.FirstOrDefaultAsync(m => m.GroupId == id && m.ApplicationUserId == userGuidId);

                viewModel = new GroupDetailsViewModel()
                {
                    Id = group.Id.ToString(),
                    Name = group.Name,
                    Description = group.Description,
                    Location = group.Location,
                    CreatedDate = group.CreatedDate.ToString(EntityValidationConstants.Group.ReleaseDateFormat),
                    MembersCount = membersCount,
                    IsFollowing = isFollowing == null ? false : true,
                    Events = events,
                    AdminId = group.AdminId.ToString(),
                    UserId = userGuidId.ToString()
                };
            }

            return viewModel;
        }

        public async Task FollowGroupAsync(Guid id, Guid userGuidId)
        {
            Group? group = await this.groupRepository
                .GetByIdAsync(id);            

            if (group != null && group.IsDeleted == false && await membershipRepository.FirstOrDefaultAsync(m => m.GroupId == id && m.ApplicationUserId == userGuidId) == null!)
            {
                Membership newMembership = new Membership()
                {
                    JoinDate = DateTime.Now,
                    ApplicationUserId = userGuidId,
                    GroupId = id
                };

                await this.membershipRepository.AddAsync(newMembership);
            }
        }

        public async Task UnFollowGroupAsync(Guid id, Guid userGuidId)
        {
            await this.membershipRepository.DeleteAsync(await this.membershipRepository.FirstOrDefaultAsync(m => m.ApplicationUserId == userGuidId && m.GroupId == id));
        }

        public async Task<IEnumerable<GroupIndexViewModel>> GetAllAdminGroupsAsync(Guid userId)
        {
            IEnumerable<GroupIndexViewModel> groups = await this.groupRepository
                .GetAllAttached()
                .Where(g => g.IsDeleted == false && g.AdminId == userId)
                .Select(c => new GroupIndexViewModel()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Location = c.Location,
                    Description = c.Description,
                    CreatedDate = c.CreatedDate.ToString(EntityValidationConstants.Group.ReleaseDateFormat)
                })
                .OrderBy(c => c.Location)
                .ToArrayAsync();

            return groups;
        }

        public async Task<DeleteGroupViewModel?> GetGroupForDeleteByIdAsync(Guid id)
        {
            var groupToDelete = await this.groupRepository
                .GetAllAttached()
                .Where(g => g.IsDeleted == false)
                .Select(g => new DeleteGroupViewModel()
                {
                    Id = g.Id.ToString(),
                    Location = g.Location,
                    Name = g.Name
                })
                .FirstOrDefaultAsync(g => g.Id.ToLower() == id.ToString().ToLower());

            return groupToDelete;
        }

        public async Task<bool> SoftDeleteGroupAsync(Guid id)
        {
            Group? groupToDelete = await this.groupRepository
                .FirstOrDefaultAsync(g => g.Id.ToString().ToLower() == id.ToString().ToLower());
            if (groupToDelete == null)
            {
                return false;
            }

            var events = await this.eventRepository
                .GetAllAttached()
                .Where(e => !e.IsDeleted && e.GroupId.ToString() == id.ToString())
                .ToArrayAsync();

            foreach (var eventEntity in events)
            {
                eventEntity.IsDeleted = true;
                await this.eventRepository.UpdateAsync(eventEntity);
            }

            groupToDelete.IsDeleted = true;
            return await this.groupRepository.UpdateAsync(groupToDelete);
        }

        public async Task<GroupEditViewModel?> GetGroupForEditAsync(Guid id)
        {
            var group = await this.groupRepository
                .GetAllAttached()
                .Where(g => g.IsDeleted == false)
                .Select(g => new GroupEditViewModel()
                {
                    Location = g.Location,
                    Name = g.Name,
                    Description = g.Description,
                    CreatedDate = g.CreatedDate,
                    Id = g.Id.ToString()
                })
                .FirstOrDefaultAsync(g => g.Id == id.ToString());

            return group;
        }

        public async Task<bool> EditGroupAsync(GroupEditViewModel viewModel)
        {
            var group = await this.groupRepository.GetByIdAsync(Guid.Parse(viewModel.Id));

            group.Description = viewModel.Description;
            group.Location = viewModel.Location;
            group.CreatedDate = viewModel.CreatedDate;
            group.Name = viewModel.Name;

            return await this.groupRepository.UpdateAsync(group);
        }
    }
}

