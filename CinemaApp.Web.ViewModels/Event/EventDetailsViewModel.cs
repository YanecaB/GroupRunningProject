using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CinemaApp.Web.ViewModels.User;

namespace CinemaApp.Web.ViewModels.Event
{
	public class EventDetailsViewModel
	{
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Date { get; set; } = null!;

        public string Location { get; set; } = null!;

        public int Distance { get; set; }               

        public ICollection<ApplicationUserViewModel> JoinedUsers { get; set; } = new List<ApplicationUserViewModel>();
    }
}

