using System;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.FriendRequest;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Data
{
	public class FriendRequestService : BaseService, IFriendRequestService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<FriendRequest, Guid> friendRequestRepository;
        private readonly IRepository<FriendRequestNotification, Guid> friendRequestNotificationRepository;

        private readonly INotificationService notificationService;

        public FriendRequestService(UserManager<ApplicationUser> userManager,
            IRepository<FriendRequest, Guid> friendRequestRepository,
            INotificationService notificationService,
            IRepository<FriendRequestNotification, Guid> friendRequestNotificationRepository)
        {
            this.userManager = userManager;
            this.friendRequestRepository = friendRequestRepository;
            this.notificationService = notificationService;
            this.friendRequestNotificationRepository = friendRequestNotificationRepository;
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

        public async Task<bool> ConfirmFriendRequestAsync(ReplyOnFriendRequestViewModel viewModel)
        {
            var currentUser = await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName == viewModel.CurrentUserUsername);
            var sender = await this.userManager.Users.FirstOrDefaultAsync(u => u.UserName == viewModel.SenderUsername);

            if (currentUser == null || sender == null)
            {
                return false;
            }

            currentUser.Friends.Add(sender);
            sender.Friends.Add(currentUser);

            // todo: Make message that you are friends now :)

            await this.friendRequestRepository.SaveChangesAsync();
            
            await this.notificationService.GenerateConfirmedRequestNotificationsAsync(currentUser, sender);

            await DeleteFriendRequestAsync(viewModel);

            return true;
        }

        public async Task<bool> DeleteFriendRequestAsync(ReplyOnFriendRequestViewModel viewModel)
        {
            var deleteFriendRequest = await this.friendRequestRepository
                .FirstOrDefaultAsync(fr => fr.Receiver.UserName == viewModel.CurrentUserUsername && fr.Sender.UserName == viewModel.SenderUsername
                && fr.IsDeleted == false);

            var deleteNotification = await this.friendRequestNotificationRepository
                .FirstOrDefaultAsync(frn => frn.FriendRequest == deleteFriendRequest && frn.IsDeleted == false);

            if (deleteFriendRequest == null || deleteNotification == null)
            {
                return false;
            }

            deleteFriendRequest.IsDeleted = true;
            deleteNotification.IsDeleted = true;

            await this.friendRequestRepository.SaveChangesAsync();

            return true;
        }
    }
}

