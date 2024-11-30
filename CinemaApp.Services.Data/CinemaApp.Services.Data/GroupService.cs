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
        private readonly IRepository<Group, Guid> groupRunningRepository;

        public GroupService(IRepository<Group, Guid> groupRunningRepository)
        {
            this.groupRunningRepository = groupRunningRepository;
        }

        public async Task<IEnumerable<GroupIndexViewModel>> IndexGetAllAsync()
        {
            IEnumerable<GroupIndexViewModel> groups = await this.groupRunningRepository
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

            await groupRunningRepository.AddAsync(newGroup);
        }
    }
}

