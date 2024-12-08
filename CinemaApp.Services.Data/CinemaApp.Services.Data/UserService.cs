using System;
using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Data
{
	public class UserService : BaseService, IUserService
	{
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<Membership, object> membershipRepository;
        private readonly IRepository<Event, Guid> eventRepository;
        private readonly IRepository<ApplicationUserEvent, object> userEventRepository;

        public UserService(UserManager<ApplicationUser> userManager,
            IRepository<Membership, object> membershipRepository,
            IRepository<Event, Guid> eventRepository,
            IRepository<ApplicationUserEvent, object> userEventRepository)           
        {
            this.userManager = userManager;
            this.membershipRepository = membershipRepository;
            this.eventRepository = eventRepository;
            this.userEventRepository = userEventRepository;
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
    }
}
