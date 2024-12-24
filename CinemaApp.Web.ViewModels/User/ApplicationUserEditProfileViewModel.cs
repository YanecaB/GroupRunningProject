using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Web.ViewModels.User
{
	public class ApplicationUserEditProfileViewModel
	{        
        public Guid Id { get; set; }

        [Required]

        public string Username { get; set; } = null!;

        public string? Bio { get; set; }

        public string? Email { get; set; }

        public string? ProfilePicturePath { get; set; }
    }
}

