using System;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Schedule;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Data
{
    public class ScheduleService : IScheduleService
    {        
        private readonly IRepository<Event, Guid> eventRepository;
        private readonly IRepository<ApplicationUserEvent, object> userEventRepository;

        public ScheduleService(IRepository<Event, Guid> eventRepository,
            IRepository<ApplicationUserEvent, object> userEventRepository)
        {
            this.eventRepository = eventRepository;
            this.userEventRepository = userEventRepository;
        }

        public async Task<MyScheduleIndexViewModel> GetMyScheduleByIdAsync(Guid id)
        {
            MyScheduleIndexViewModel schedule = new MyScheduleIndexViewModel()
            {
                //TODO: FINISH the getting of my events and joined events

                JoinedEvents = await this.userEventRepository
                    .GetAllAttached()
                    .Include(ue => ue.Event)
                    .Where(ue => ue.ApplicationUserId == id)
                    .Select(ue => new EventViewModel()
                    {
                        Id = ue.EventId.ToString(),
                        Title = ue.Event.Title
                    })
                    .ToArrayAsync(),
                MyEvents = await this.eventRepository
                    .GetAllAttached()
                    .Where(e => e.IsDeleted == false && e.OrganizerId == id)
                    .Select(e => new EventViewModel()
                    {
                        Id = e.Id.ToString(),
                        Title = e.Title
                    })
                    .ToArrayAsync()
            };

            return schedule;
        }
    }
}

