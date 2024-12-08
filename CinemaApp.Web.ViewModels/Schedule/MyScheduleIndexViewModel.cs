using System;
using CinemaApp.Web.ViewModels.Event;

namespace CinemaApp.Web.ViewModels.Schedule
{
	public class MyScheduleIndexViewModel
	{
        public string? Id { get; set; }

        public IEnumerable<EventViewModel> MyEvents { get; set; } = null!;

        public IEnumerable<EventViewModel> JoinedEvents { get; set; } = null!;
    }
}

