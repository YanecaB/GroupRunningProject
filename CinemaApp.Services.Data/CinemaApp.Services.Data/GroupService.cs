using System;
using System.Security.Claims;
using CinemaApp.Common;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Group;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CinemaApp.Services.Data
{
    public class GroupService : BaseService, IGroupService
    {
        private readonly IRepository<Group, Guid> groupRepository;

        private readonly IRepository<Membership, object> membershipRepository;

        public GroupService(IRepository<Group, Guid> groupRepository,
            IRepository<Membership, object> membershipRepository)
        {
            this.groupRepository = groupRepository;
            this.membershipRepository = membershipRepository;
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
        }

        public async Task<GroupDetailsViewModel> GetGroupDetailsByIdAsync(Guid id)
        {
            Group? group = await this.groupRepository
                .GetByIdAsync(id);

            GroupDetailsViewModel? viewModel = null;

            if (group != null && group.IsDeleted == false)
            {
                viewModel = new GroupDetailsViewModel()
                {
                    Id = group.Id.ToString(),
                    Name = group.Name,
                    Description = group.Description,
                    Location = group.Location,
                    CreatedDate = group.CreatedDate.ToString(EntityValidationConstants.Group.ReleaseDateFormat),
                    MembersCount = group.Memberships.Count()
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
                    GroupId = group.Id
                };

                await this.membershipRepository.AddAsync(newMembership);
            }
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
    }
}

