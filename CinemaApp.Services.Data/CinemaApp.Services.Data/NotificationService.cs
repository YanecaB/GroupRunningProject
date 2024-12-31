using System;
using System.Globalization;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Notification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static CinemaApp.Common.EntityValidationConstants.Notification;

namespace CinemaApp.Services.Data
{
    public class NotificationService : BaseService, INotificationService
    {        
        private readonly IRepository<Event, Guid> eventRepository;
        private readonly IRepository<ApplicationUserEvent, object> userEventRepository;        
        private readonly IRepository<FriendRequest, Guid> friendRequestRepository;        

        private readonly IRepository<FriendRequestNotification, Guid> friendRequestNotificationRepository;
        private readonly IRepository<EventNotification, Guid> eventNotificationRepository;
        private readonly IRepository<ConfirmedRequestNotification, Guid> confirmedRequestNotificationRepository;

        public NotificationService(IRepository<Event, Guid> eventRepository,
            IRepository<ApplicationUserEvent, object> userEventRepository,            
            IRepository<FriendRequest, Guid> friendRequestRepository,
            IRepository<FriendRequestNotification, Guid> friendRequestNotificationRepository,
            IRepository<EventNotification, Guid> eventNotificationRepository,
            IRepository<ConfirmedRequestNotification, Guid> confirmedRequestNotificationRepository)
        {
            this.eventRepository = eventRepository;
            this.userEventRepository = userEventRepository;            
            this.friendRequestRepository = friendRequestRepository;
            this.friendRequestNotificationRepository = friendRequestNotificationRepository;
            this.eventNotificationRepository = eventNotificationRepository;
            this.confirmedRequestNotificationRepository = confirmedRequestNotificationRepository;            
        }

        public async Task GenerateConfirmedRequestNotificationsAsync(ApplicationUser currentUser, ApplicationUser receiver)
        {            
            var confirmedRequestNotification = new ConfirmedRequestNotification()
            {
                Id = Guid.NewGuid(),
                Message = string.Format(MessageForConfirmedRequests, currentUser.UserName),
                NewFriend = currentUser,
                User = receiver                
            };

            await this.confirmedRequestNotificationRepository.AddAsync(confirmedRequestNotification);
        }

        public async Task GenerateEventNotificationsAsync()
        {
            var upcomingEvents = await this.eventRepository
                .GetAllAttached()
                .Where(e => e.Date.Date == DateTime.Now.AddDays(1).Date)
                .ToListAsync();

            foreach (var eventEntity in upcomingEvents)
            {
                var joinedUsers = await this.userEventRepository
                    .GetAllAttached()
                    .Where(ue => ue.EventId == eventEntity.Id)
                    .Select(ue => ue.ApplicationUserId)
                    .ToListAsync();

                foreach (var userId in joinedUsers)
                {
                    if (await this.eventNotificationRepository
                        .GetAllAttached()
                        //.OfType<EventNotification>()
                        .FirstOrDefaultAsync(n => n.EventId == eventEntity.Id && n.UserId == userId) != null)
                    {
                        continue;
                    }

                    var notification = new EventNotification
                    {
                        Id = Guid.NewGuid(),
                        Message = string.Format(MessageForEvents, eventEntity.Title),
                        Date = DateTime.Now,
                        UserId = userId,
                        EventId = eventEntity.Id                        
                    };

                    await this.eventNotificationRepository.AddAsync(notification);
                }
            }
        }

        public async Task GenerateFriendRequestNotificationsAsync(FriendRequest friendRequest)
        {
                    
            var senderName = (await this.friendRequestRepository.GetAllAttached().Include(fr => fr.Sender).FirstOrDefaultAsync(frn => frn.Id == friendRequest.Id)).Sender.UserName;

            var notification = new FriendRequestNotification
            {
                Id = Guid.NewGuid(),
                Message = string.Format(MessageForFriendRequests, friendRequest.Sender.UserName),
                Date = friendRequest.SendRequestDate,
                UserId = friendRequest.ReceiverId,
                FriendRequest = friendRequest
            };

            await this.friendRequestNotificationRepository.AddAsync(notification);            
        }

        public async Task<IEnumerable<NotificationViewModel>> GetNotificationsByUserIdAsync(Guid userId)
        {
            var eventNotifications = await this.eventNotificationRepository
                .GetAllAttached()
                //.OfType<EventNotification>()
                .Include(n => n.Event)
                .Where(n => n.UserId == userId && n.Event.IsDeleted == false)
                //.OrderByDescending(n => n.Date)
                .Select(n => new NotificationViewModel()
                {
                    Message = n.Message,
                    Date = n.Date.ToString(DateFormat),
                    Id = n.Id.ToString(),
                    EventName = n.Event.Title,
                    EventId = n.EventId.ToString()
                })                
                .ToListAsync();

            var friendRequestNotifications = await this.friendRequestNotificationRepository
                .GetAllAttached()
                //.OfType<FriendRequestNotification>()
                .Include(n => n.FriendRequest)
                .ThenInclude(fr => fr.Sender)
                .Where(n => n.UserId == userId && n.FriendRequest.IsDeleted == false && n.IsDeleted == false)
                .Select(n => new NotificationViewModel()
                {
                    Message = n.Message,
                    Date = n.Date.ToString(DateFormat),
                    Id = n.Id.ToString(),
                    SenderUserName = n.FriendRequest.Sender.UserName
                })
                .ToListAsync();

            var confirmedRequestNotifications = await this.confirmedRequestNotificationRepository
                .GetAllAttached()
                .Include(cr => cr.NewFriend)
                .Where(n => n.UserId == userId && n.IsDeleted == false)
                .Select(n => new NotificationViewModel()
                {
                    Message = n.Message,
                    Date = n.Date.ToString(DateFormat),
                    Id = n.Id.ToString(),
                    NewFriendUsername = n.NewFriend.UserName
                })
                .ToListAsync();

            return eventNotifications
                .Concat(friendRequestNotifications)
                .Concat(confirmedRequestNotifications)
                .OrderByDescending(n => DateTime.ParseExact(n.Date, DateFormat, CultureInfo.InvariantCulture));
        }
    }
}

