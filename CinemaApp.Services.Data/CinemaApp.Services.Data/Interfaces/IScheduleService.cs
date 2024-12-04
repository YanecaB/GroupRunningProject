using System;
using CinemaApp.Web.ViewModels.Schedule;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface IScheduleService
	{
        Task<MyScheduleIndexViewModel> GetMyScheduleByIdAsync(Guid id);
    }
}

