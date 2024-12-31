using System;
using CinemaApp.Data.Models;
using CinemaApp.Web.Areas.Identity.Services.Interfaces;
using CinemaApp.Web.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static CinemaApp.Common.ApplicationConstants;

using static CinemaApp.Common.EntityValidationConstants.ApplicationUser;

namespace CinemaApp.Web.Areas.Identity.Services
{
    public class AccountService : BaseService, IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment hostEnvironment;

        public AccountService(UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            this.userManager = userManager;
            this.hostEnvironment = hostEnvironment;
        }
       
        public async Task<ApplicationUserDetailsViewModel> GetDetailsByIdAsync(Guid id, Guid currentId)
        {
            var user = await userManager.Users                
                .Include(u => u.ApplicationUserEvents)
                .Include(u => u.Friends)
                .FirstOrDefaultAsync(u => u.Id.ToString() == id.ToString());

            if (user == null && currentId.ToString() != id.ToString())
            {
                return null;
            }

            var friends = user.Friends.Select(f => new ApplicationUserViewModel()
            {
                Email = f.Email,
                UserName = f.UserName,
                Id = f.Id.ToString()
            }).ToList();

            var viewModel = new ApplicationUserDetailsViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Bio = string.IsNullOrEmpty(user.Bio) ? EmptyBioMessage : user.Bio,
                IsBanned = user.IsBanned,
                UserEvents = user.ApplicationUserEvents.ToList().Count(),
                Friends = friends,
                ProfilePicturePath = user.ProfilePicturePath
            };

            return viewModel;
        }

        public async Task<ApplicationUserEditProfileViewModel> GetInfoForEditProfileByIdAsync(Guid id)
        {
            var user = await this.userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return null;
            }

            var viewModel = new ApplicationUserEditProfileViewModel
            {
                Id = user.Id.ToString(),
                Email = user.Email,
                Username = user.UserName,
                Bio = string.IsNullOrEmpty(user.Bio) ? EmptyBioMessage : user.Bio,
                ExistingProfilePicturePath = user.ProfilePicturePath
            };

            return viewModel;
        }

        public async Task<bool> EditProfileByIdAsync(ApplicationUserEditProfileViewModel viewModel)
        {
            var user = await userManager.Users
               .FirstOrDefaultAsync(u => u.Id.ToString() == viewModel.Id);

            if (user == null)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(viewModel.Bio))
            {
                user.Bio = viewModel.Bio;
            }

            if (this.userManager.Users.Any(u => u.UserName == viewModel.Username) && user.UserName != viewModel.Username)
            {
                return false;
            }

            user.UserName = viewModel.Username;
            
            if (viewModel.ProfilePicturePath != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(viewModel.ProfilePicturePath.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    throw new InvalidOperationException("Invalid file type.");
                }

                var uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "images", "profile-pictures");
                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(uploadsFolder, fileName);

                if (!string.IsNullOrEmpty(user.ProfilePicturePath) && user.ProfilePicturePath != DefaultProfileImgUrl)
                {
                    var oldFilePath = Path.Combine(hostEnvironment.WebRootPath, user.ProfilePicturePath.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }
                
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.ProfilePicturePath.CopyToAsync(stream);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error saving file: {ex.Message}");
                }

                user.ProfilePicturePath = $"/images/profile-pictures/{fileName}";
            }

            var result = await userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}

