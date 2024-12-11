using System;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.RankList;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Data
{
    public class RankListService : BaseService, IRankListService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<Event, Guid> eventRepository;
        private readonly IRepository<ApplicationUserEvent, object> userEventRepository;

        public RankListService(UserManager<ApplicationUser> userManager,
            IRepository<Event, Guid> eventRepository,
            IRepository<ApplicationUserEvent, object> userEventRepository)
        {
            this.userManager = userManager;
            this.eventRepository = eventRepository;
            this.userEventRepository = userEventRepository;
        }

        public async Task<ICollection<RankListUserViewModel>> GetAllUsersOrderedByRunnedDistanceAsync()
        {
            return await this.userManager.Users
                .Where(u => u.IsBanned == false)
                .Select(u => new RankListUserViewModel()
                {
                    Id = u.Id.ToString(),
                    UserName = u.UserName,
                    RunnedDistance = u.RunnedDistance
                })
                .OrderByDescending(u => u.RunnedDistance)
                .ToListAsync();
        }

        /*

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
                    if (await this.notificationRepository.FirstOrDefaultAsync(n => n.EventId == eventEntity.Id && n.UserId == userId) != null)
                    {
                        continue;
                    }

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
         
         */
        public async Task DeletePassedEventsAndRunnedDistanceToTheParticipants()
        {
            var passedEvents = await this.eventRepository
                .GetAllAttached()
                .Include(e => e.UsersEvents)
                .ThenInclude(ue => ue.ApplicationUser)
                .Where(e => e.Date < DateTime.Now && e.IsDeleted == false)
                .ToListAsync();

            foreach (var passedEvent in passedEvents)
            {
                foreach (var userEvent in passedEvent.UsersEvents)
                {
                    if (userEvent.ApplicationUser.IsBanned == false)
                    {
                        userEvent.ApplicationUser.RunnedDistance += passedEvent.Distance;
                        await this.userManager.UpdateAsync(userEvent.ApplicationUser);
                    }
                }

                passedEvent.IsDeleted = true;
                await this.eventRepository.UpdateAsync(passedEvent);
            }
        }
    }
}

