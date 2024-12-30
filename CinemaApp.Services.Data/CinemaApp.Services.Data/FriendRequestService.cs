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

        private readonly INotificationService notificationService;

        public FriendRequestService(UserManager<ApplicationUser> userManager,
            IRepository<FriendRequest, Guid> friendRequestRepository,
            INotificationService notificationService)
        {
            this.userManager = userManager;
            this.friendRequestRepository = friendRequestRepository;
            this.notificationService = notificationService;
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

            var newFriendRequest = new FriendRequest()
            {
                Id = Guid.NewGuid(),
                Receiver = receiver,
                SenderId = senderId
            };

            await this.friendRequestRepository.AddAsync(newFriendRequest);

            await this.notificationService.GenerateFriendRequestNotificationsAsync(newFriendRequest);

            return true;
        }
    }
}

