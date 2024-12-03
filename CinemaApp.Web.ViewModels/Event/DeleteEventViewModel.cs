using System;
namespace CinemaApp.Web.ViewModels.Event
{
	public class DeleteEventViewModel
	{
        public string Id { get; set; } = null!;

        public string? Title { get; set; }

        public string? Location { get; set; }
    }
}

