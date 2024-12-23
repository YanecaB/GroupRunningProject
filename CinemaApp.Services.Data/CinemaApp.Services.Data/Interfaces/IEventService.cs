﻿using System;
using CinemaApp.Web.ViewModels.Event;
using CinemaApp.Web.ViewModels.Group;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface IEventService
	{
        Task<(IEnumerable<EventIndexViewModel>, int)> IndexGetAllAsync(Guid? userId,
            string? searchQuery = null,
            int pageNumber = 1,
            int pageSize = 2); // the pageSize is set to 2 for easier testing of the Pagination :)

        Task<IEnumerable<EventIndexViewModel>> GetAllAdminEventsAsync(Guid userId);

        Task AddEventAsync(EventCreateViewModel viewModel, Guid adminId);

        Task JoinEventAsync(Guid eventId, Guid userId);

        Task UnjoinEventAsync(Guid eventId, Guid userId);

        Task<EventDetailsViewModel> GetEventDetailsByIdAsync(Guid id, Guid userGuidId);

        Task<EventEditViewModel?> GetEventForEditAsync(Guid id);

        Task<bool> EditEventAsync(EventEditViewModel viewModel);

        Task<bool> RemoveAnttendeeAsync(Guid eventId, Guid anttendee);

        Task<bool> SoftDeleteEventAsync(Guid id);

        Task<DeleteEventViewModel?> GetEventForDeleteByIdAsync(Guid id);

        Task<bool> EventExistsByIdAsync(Guid id);        
    }
}

 