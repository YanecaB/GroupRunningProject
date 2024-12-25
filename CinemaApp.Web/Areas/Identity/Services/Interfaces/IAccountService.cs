using System;
using CinemaApp.Web.ViewModels.User;

namespace CinemaApp.Web.Areas.Identity.Services.Interfaces
{
	public interface IAccountService
	{
		Task<ApplicationUserDetailsViewModel> GetDetailsByIdAsync(Guid id, Guid currentId);

		Task<ApplicationUserEditProfileViewModel> GetInfoForEditProfileByIdAsync(Guid id);

		Task<bool> EditProfileByIdAsync(ApplicationUserEditProfileViewModel viewModel);
	}
}

