using System;
namespace CinemaApp.Web.ViewModels.Event
{
	public class EventIndexViewModel
	{
		public string Id { get; set; } = null!;

		public string Title { get; set; } = null!;

		public string GroupName { get; set; } = null!;

		public string Date { get; set; } = null!;

		public int JoinedUsers { get; set; }

		public string AdminId { get; set; } = null!;
	}
}

