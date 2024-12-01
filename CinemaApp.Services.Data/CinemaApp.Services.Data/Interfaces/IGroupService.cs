using System;
using CinemaApp.Web.ViewModels.Group;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface IGroupService
	{
        Task<IEnumerable<GroupIndexViewModel>> IndexGetAllAsync();

        Task AddGroupAsync(GroupCreateViewModel viewModel, Guid adminId);

        Task<GroupDetailsViewModel?> GetGroupDetailsByIdAsync(Guid id, Guid userGuidId);

        Task FollowGroupAsync(Guid id, Guid userGuidId);

        Task UnFollowGroupAsync(Guid id, Guid userGuidId);

        Task<IEnumerable<GroupIndexViewModel>> GetAllAdminGroupsAsync(Guid userId);

        Task<DeleteGroupViewModel?> GetGroupForDeleteByIdAsync(Guid id);

        Task<bool> SoftDeleteGroupAsync(Guid id);

        Task<GroupEditViewModel?> GetGroupForEditAsync(Guid id);

        Task<bool> EditGroupAsync(GroupEditViewModel viewModel);
    }
}

