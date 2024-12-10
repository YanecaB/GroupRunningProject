using System;
using CinemaApp.Web.ViewModels.Group;
using CinemaApp.Web.ViewModels.User;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface IGroupService
	{
        Task<IEnumerable<GroupIndexViewModel>> IndexGetAllAsync(string? searchQuery = null);

        Task AddGroupAsync(GroupCreateViewModel viewModel, Guid adminId);

        Task<GroupDetailsViewModel?> GetGroupDetailsByIdAsync(Guid id, Guid userGuidId);

        Task FollowGroupAsync(Guid id, Guid userGuidId);

        Task UnFollowGroupAsync(Guid id, Guid userGuidId);

        Task<IEnumerable<GroupIndexViewModel>> GetAllAdminGroupsAsync(Guid userId);

        Task<DeleteGroupViewModel?> GetGroupForDeleteByIdAsync(Guid id);

        Task<bool> SoftDeleteGroupAsync(Guid id);

        Task<GroupEditViewModel?> GetGroupForEditAsync(Guid id);

        Task<bool> EditGroupAsync(GroupEditViewModel viewModel);

        Task<bool> RemoveFollowerAsync(Guid groupId, Guid followerId);

        Task<bool> GroupExistsByIdAsync(Guid id);
    }
}

