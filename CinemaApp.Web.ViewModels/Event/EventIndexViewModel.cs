using System;
namespace CinemaApp.Web.ViewModels.Event
{
	public class EventIndexViewModel
	{
		public Guid Id { get; set; }

		public string Title { get; set; } = null!;

		public string GroupName { get; set; } = null!;
	}
}

