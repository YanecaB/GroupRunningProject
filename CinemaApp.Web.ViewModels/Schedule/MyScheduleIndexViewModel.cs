using System;
using CinemaApp.Web.ViewModels.Event;

namespace CinemaApp.Web.ViewModels.Schedule
{
	public class MyScheduleIndexViewModel
	{
        public IEnumerable<EventIndexViewModel> MyEvents { get; set; } = new List<EventIndexViewModel>();

        public IEnumerable<EventIndexViewModel> JoinedEvents { get; set; } = new List<EventIndexViewModel>();
    }
}

