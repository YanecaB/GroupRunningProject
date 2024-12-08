using System;
namespace CinemaApp.Web.ViewModels.User
{
	public class ApplicationUserDetailsViewModel
	{
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public bool IsBanned { get; set; }

        public ICollection<string> Memberships { get; set; } = null!;

        public ICollection<string> UserEvents { get; set; } = null!;

        public ICollection<string> Notifications { get; set; } = null!;
    }
}

