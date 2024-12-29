using System;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Data
{
	public class FriendRequestService : BaseService, IFriendRequestService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<FriendRequest, Guid> friendRequestRepository;

        public FriendRequestService(UserManager<ApplicationUser> userManager,
            IRepository<FriendRequest, Guid> friendRequestRepository)
        {
            this.userManager = userManager;
            this.friendRequestRepository = friendRequestRepository;
        }

        public async Task<bool> SendFriendRequestAsync(string? username, Guid senderId)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            var receiver = await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());

            if (receiver == null || receiver.IsBanned)
            {
                return false;
            }

            await this.friendRequestRepository.AddAsync(new FriendRequest()
            {
                Id = Guid.NewGuid(),
                Receiver = receiver,
                SenderId = senderId
            });

            return true;
        }
    }
}

