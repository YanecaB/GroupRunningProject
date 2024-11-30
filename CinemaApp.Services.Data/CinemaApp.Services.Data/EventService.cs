using System;
using CinemaApp.Common;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Event;
using CinemaApp.Web.ViewModels.Group;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Services.Data
{
    public class EventService : BaseService, IEventService
    {
        private readonly IRepository<Event, Guid> eventRepository;        

        public EventService(IRepository<Event, Guid> eventRepository)            
        {
            this.eventRepository = eventRepository;            
        }
       
        public async Task<IEnumerable<EventIndexViewModel>> IndexGetAllAsync()
        {
            IEnumerable<EventIndexViewModel> events = await this.eventRepository
                .GetAllAttached()
                .Where(g => g.IsDeleted == false)
                .Select(c => new EventIndexViewModel()
                {
                    Id = c.Id.ToString(),
                    Title = c.Title,                                        
                    Date = c.Date.ToString(EntityValidationConstants.Event.DateFormat),
                    GroupName = c.Group.Name
                })                
                .ToArrayAsync();

            return events;
        }

        public async Task<IEnumerable<EventIndexViewModel>> GetAllAdminEventsAsync(Guid userId)
        {
            IEnumerable<EventIndexViewModel> events = await this.eventRepository
                .GetAllAttached()
                .Where(g => g.IsDeleted == false && g.OrganizerId == userId)
                .Select(c => new EventIndexViewModel()
                {
                    Id = c.Id.ToString(),
                    Title = c.Title,
                    Date = c.Date.ToString(EntityValidationConstants.Event.DateFormat),
                    GroupName = c.Group.Name
                })
                .ToArrayAsync();

            return events;
        }
    }
}

