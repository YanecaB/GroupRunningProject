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
        private readonly IRepository<ApplicationUserEvent, object> userEventRepository;

        public EventService(IRepository<Event, Guid> eventRepository,
            IRepository<ApplicationUserEvent, object> userEventRepository)            
        {
            this.eventRepository = eventRepository;
            this.userEventRepository = userEventRepository;
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

        public async Task AddEventAsync(EventCreateViewModel viewModel, Guid adminId)
        {
            Event newEvent = new Event()
            {
                Id = Guid.NewGuid(),
                Title = viewModel.Title,
                Date = viewModel.Date,
                Distance = viewModel.Distance,
                Location = viewModel.Location,
                Description = viewModel.Description,
                OrganizerId = adminId,
                GroupId = Guid.Parse(viewModel.GroupId)
            };

            await eventRepository.AddAsync(newEvent);

            await userEventRepository.AddAsync(new ApplicationUserEvent()
            {
                ApplicationUserId = adminId,
                EventId = newEvent.Id
            });
        }
    }
}

