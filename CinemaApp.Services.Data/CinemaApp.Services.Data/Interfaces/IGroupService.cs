using System;
using CinemaApp.Web.ViewModels.Group;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface IGroupService
	{
        Task<IEnumerable<GroupIndexViewModel>> IndexGetAllAsync();

        Task AddGroupAsync(GroupCreateViewModel viewModel, Guid adminId);

        Task<GroupDetailsViewModel> GetGroupDetailsByIdAsync(Guid id);

        Task FollowGroupAsync(Guid id, Guid userGuidId);
    }
}

