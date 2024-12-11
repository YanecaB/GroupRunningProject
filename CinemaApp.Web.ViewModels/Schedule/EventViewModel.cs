using System;
namespace CinemaApp.Web.ViewModels.Schedule
{
	public class EventViewModel
	{
		public string Id { get; set; } = null!;

		public string Title { get; set; } = null!;

		public bool IsPassed { get; set; }
	}
}

