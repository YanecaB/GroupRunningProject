using System;
using CinemaApp.Web.ViewModels.Event;
using CinemaApp.Web.ViewModels.Group;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface IEventService
	{
        Task<IEnumerable<EventIndexViewModel>> IndexGetAllAsync();

        Task<IEnumerable<EventIndexViewModel>> GetAllAdminEventsAsync(Guid userId);

        Task AddEventAsync(EventCreateViewModel viewModel, Guid adminId);
    }
}

