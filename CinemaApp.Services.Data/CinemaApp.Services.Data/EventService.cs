using System;
using CinemaApp.Common;
using CinemaApp.Data.Models;
using CinemaApp.Data.Repository.Interfaces;
using CinemaApp.Services.Data.Interfaces;
using CinemaApp.Web.ViewModels.Event;
using CinemaApp.Web.ViewModels.Group;
using CinemaApp.Web.ViewModels.User;
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

        public async Task JoinEventAsync(Guid eventId, Guid userId)
        {
            if (await this.userEventRepository.FirstOrDefaultAsync(ue => ue.EventId == eventId && ue.ApplicationUserId == userId) == null)
            {
                await this.userEventRepository.AddAsync(new ApplicationUserEvent()
                {
                    ApplicationUserId = userId,
                    EventId = eventId
                });
            }            
        }

        public async Task<EventDetailsViewModel> GetEventDetailsByIdAsync(Guid id, Guid userGuidId)
        {
            var eventEntity = (await this.eventRepository
                .GetAllAttached()
                .Where(e => e.IsDeleted == false)
                .Include(e => e.UsersEvents)
                .ThenInclude(ue => ue.ApplicationUser)
                .ToArrayAsync())
                .FirstOrDefault(e => e.Id == id);            

            EventDetailsViewModel? viewModel = null;
           
            if (eventEntity != null && eventEntity.IsDeleted == false)
            {
                var users = eventEntity.UsersEvents
                 .Where(ue => ue.ApplicationUser != null)
                 .Select(ue => new ApplicationUserViewModel()
                 {
                     Id = ue.ApplicationUserId.ToString(),
                     UserName = ue.ApplicationUser.UserName,
                     Email = ue.ApplicationUser.Email
                 })
                 .ToList();

                viewModel = new EventDetailsViewModel()
                {
                    Id = eventEntity.Id.ToString(),
                    Title = eventEntity.Title,
                    Date = eventEntity.Date.ToString(EntityValidationConstants.Event.DateFormat),
                    Location = eventEntity.Location,
                    Description = eventEntity.Description,
                    Distance = eventEntity.Distance,
                    JoinedUsers = users
                };
            }

            return viewModel;
        }

        public async Task<EventEditViewModel?> GetEventForEditAsync(Guid id)
        {
            Event? eventEntity = (await this.eventRepository
                .GetAllAttached()
                .Where(e => e.IsDeleted == false)
                .Include(e => e.UsersEvents)
                .ThenInclude(ue => ue.ApplicationUser)
                .ToArrayAsync())
                .FirstOrDefault(e => e.Id == id);

            EventEditViewModel? viewModel = null!;

            if (eventEntity != null && eventEntity.IsDeleted == false)
            {                
                viewModel = new EventEditViewModel()
                {
                    Id = id.ToString(),
                    Description = eventEntity.Description,
                    Distance = eventEntity.Distance,
                    Location = eventEntity.Location,
                    Title = eventEntity.Title,
                    Date = eventEntity.Date,
                    JoinedUsers = eventEntity.UsersEvents.Select(ue => new ApplicationUserViewModel()
                    {
                        Id = ue.ApplicationUserId.ToString(),
                        Email = ue.ApplicationUser.Email,
                        UserName = ue.ApplicationUser.UserName
                    })
                    .ToList()
                };
            }                                

            return viewModel;
        }

        public async Task<bool> EditEventAsync(EventEditViewModel viewModel)
        {
            var eventEntity = await this.eventRepository.GetByIdAsync(Guid.Parse(viewModel.Id));

            eventEntity.Description = viewModel.Description;
            eventEntity.Location = viewModel.Location;
            eventEntity.Date = viewModel.Date;
            eventEntity.Title = viewModel.Title;
            eventEntity.Distance = viewModel.Distance;

            return await this.eventRepository.UpdateAsync(eventEntity);
        }

        public async Task<bool> RemoveAnttendeeAsync(Guid eventId, Guid anttendee)
        {            
            return await this.userEventRepository
                .DeleteAsync(await this.userEventRepository.
                FirstOrDefaultAsync(ue => ue.EventId == eventId && ue.ApplicationUserId == anttendee));
        }

        public async Task<bool> SoftDeleteEventAsync(Guid id)
        {
            Event? eventToDelete = await this.eventRepository                
                .FirstOrDefaultAsync(e => e.Id.ToString().ToLower() == id.ToString().ToLower());

            if (eventToDelete == null)
            {
                return false;
            }

            eventToDelete.IsDeleted = true;
            return await this.eventRepository.UpdateAsync(eventToDelete);
        }

        public async Task<DeleteEventViewModel?> GetEventForDeleteByIdAsync(Guid id)
        {
            var eventToDelete = await this.eventRepository
                .GetAllAttached()
                .Where(g => g.IsDeleted == false)
                .Select(g => new DeleteEventViewModel()
                {
                    Id = g.Id.ToString(),
                    Location = g.Location,
                    Title = g.Title
                })
                .FirstOrDefaultAsync(g => g.Id.ToLower() == id.ToString().ToLower());

            return eventToDelete;
        }
    }
}

