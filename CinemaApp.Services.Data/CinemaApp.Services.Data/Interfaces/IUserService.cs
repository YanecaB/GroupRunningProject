using System;
using CinemaApp.Web.ViewModels.User;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface IUserService
	{
		Task<IEnumerable<UserManagementViewModel>> GetAllUsersAsync();

		Task<bool> BanUserByIdAsync(Guid userId);

        Task<bool> UnBanUserByIdAsync(Guid userId);

        Task<bool> UserExistsByIdAsync(Guid id);

		Task<UserProfileDetailsViewModel> GetUserProfileDetails(string username, Guid currentUserId);
    }
}

