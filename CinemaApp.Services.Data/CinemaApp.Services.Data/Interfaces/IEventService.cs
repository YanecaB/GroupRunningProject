using System;
using CinemaApp.Web.ViewModels.Event;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface IEventService
	{
        Task<IEnumerable<EventIndexViewModel>> IndexGetAllAsync();
    }
}

