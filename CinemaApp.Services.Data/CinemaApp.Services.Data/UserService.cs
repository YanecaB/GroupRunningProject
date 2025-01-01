using System;
using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CinemaApp.Web.ViewModels.Group;
using CinemaApp.Common;

namespace CinemaApp.Services.Data
{
	public class UserService : BaseService, IUserService
	{
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<Group, Guid> groupRepository;
        private readonly IRepository<FriendRequest, Guid> friendRequestRepository;

        public UserService(UserManager<ApplicationUser> userManager,
            IRepository<Group, Guid> groupRepository,
            IRepository<FriendRequest, Guid> friendRequestRepository)
        {
            this.userManager = userManager;
            this.groupRepository = groupRepository;
            this.friendRequestRepository = friendRequestRepository;
        }
       
        public async Task<IEnumerable<UserManagementViewModel>> GetAllUsersAsync()
        {
            var allUsersViewModel = await this.userManager.Users
                //.Where(u => u.IsBanned == false)
                .Select(u => new UserManagementViewModel()
                {
                    Id = u.Id.ToString(),
                    Email = u.Email,
                    UserName = u.UserName,
                    IsBanned = u.IsBanned
                })
                .ToArrayAsync();

            return allUsersViewModel;
        }

        public async Task<bool> BanUserByIdAsync(Guid userId)
        {
            var userToBan = await this.userManager.FindByIdAsync(userId.ToString());

            if (userToBan == null)
            {
                return false;
            }
            
            userToBan.IsBanned = true;
            
            var result = await this.userManager.UpdateAsync(userToBan);

            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UnBanUserByIdAsync(Guid userId)
        {
            var userToBan = await this.userManager.FindByIdAsync(userId.ToString());

            if (userToBan == null)
            {
                return false;
            }

            userToBan.IsBanned = false;

            var result = await this.userManager.UpdateAsync(userToBan);

            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UserExistsByIdAsync(Guid id)
        {
            ApplicationUser? user = await this.userManager
                .FindByIdAsync(id.ToString());

            return user != null;
        }

        public async Task<UserProfileDetailsViewModel> GetUserProfileDetails(string username, Guid currentUserId)
        {
            ApplicationUser? user = await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
            var currentUser = await this.userManager.Users.FirstAsync(u => u.Id == currentUserId);

            if (user == null || currentUser == null)
            {
                return null;
            }

            // todo: check if there is a friend request with isDeleted == false and if yes, add true in the ViewModel
            var sentFriendRequest = await this.friendRequestRepository.FirstOrDefaultAsync(fr => fr.Sender == currentUser && fr.Receiver == user && fr.IsDeleted == false);

            bool sendButtonOrDeleteButton = true;
            if (sentFriendRequest != null)
            {
                sendButtonOrDeleteButton = false;
            }

            return new UserProfileDetailsViewModel()
            {
                Username = user.UserName,
                Bio = user.Bio,
                SendButtonOrDeleteButton = sendButtonOrDeleteButton,
                IsBanned = user.IsBanned,
                ProfilePicturePath = user.ProfilePicturePath,
                AdminGroups = await groupRepository.GetAllAttached().Where(g => g.IsDeleted == false && g.ApplicationUser.UserName == username).Select(g => new GroupIndexViewModel()
                {
                    Id = g.Id.ToString(),
                    Description = g.Description,
                    Location = g.Location,
                    Name = g.Name,
                    CreatedDate = g.CreatedDate.ToString(EntityValidationConstants.Group.ReleaseDateFormat)
                }).ToListAsync()
            };
        }
    }
}
