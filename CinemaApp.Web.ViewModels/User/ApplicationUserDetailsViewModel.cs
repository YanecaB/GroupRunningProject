using System;
namespace CinemaApp.Web.ViewModels.User
{
	public class ApplicationUserDetailsViewModel
	{
        public Guid Id { get; set; }

        public string Username { get; set; } = null!;

        public string? Bio { get; set; }

        public string? Email { get; set; }

        public bool IsBanned { get; set; }

        public int UserEvents { get; set; } = 0;

        public ICollection<ApplicationUserViewModel> Friends { get; set; } = null!;
    }
}

