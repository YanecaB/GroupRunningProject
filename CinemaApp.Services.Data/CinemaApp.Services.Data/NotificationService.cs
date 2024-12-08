using System;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Notification;
using Microsoft.EntityFrameworkCore;
using static CinemaApp.Common.EntityValidationConstants.Notification;

namespace CinemaApp.Services.Data
{
    public class NotificationService : BaseService, INotificationService
    {
        private readonly IRepository<Event, Guid> eventRepository;
        private readonly IRepository<ApplicationUserEvent, object> userEventRepository;
        private readonly IRepository<Notification, Guid> notificationRepository;

        public NotificationService(IRepository<Event, Guid> eventRepository,
            IRepository<ApplicationUserEvent, object> userEventRepository,
            IRepository<Notification, Guid> notificationRepository)
        {
            this.eventRepository = eventRepository;
            this.userEventRepository = userEventRepository;
            this.notificationRepository = notificationRepository;
        }

        public async Task GenerateNotificationsAsync()
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
                    var notification = new Notification
                    {
                        Id = Guid.NewGuid(),
                        Message = string.Format(Message, eventEntity.Title),
                        Date = DateTime.Now,
                        UserId = userId,
                        EventId = eventEntity.Id
                    };

                    await this.notificationRepository.AddAsync(notification);
                }
            }
        }

        public async Task<IEnumerable<NotificationViewModel>> GetNotificationsByUserIdAsync(Guid userId)
        {
            var allNotificationOfTheCurrentUser = await this.notificationRepository
                .GetAllAttached()
                .Include(n => n.Event)
                .Where(n => n.UserId == userId)
                .Select(n => new NotificationViewModel()
                {
                    Message = n.Message,
                    Date = n.Date.ToString(DateFormat),
                    Id = n.Id.ToString(),
                    EventName = n.Event.Title,
                    EventId = n.EventId.ToString()
                })
                .ToArrayAsync();

            return allNotificationOfTheCurrentUser;
        }
    }
}

