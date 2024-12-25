using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using static CinemaApp.Common.EntityValidationConstants.ApplicationUser;

namespace CinemaApp.Web.ViewModels.User
{
	public class ApplicationUserEditProfileViewModel
	{
        public string Id { get; set; } = null!;

        [Required]
        [MinLength(UsernameMinLength)]
        [MaxLength(UsernameMaxLength)]
        public string Username { get; set; } = null!;

        [MinLength(BioMinLength)]
        [MaxLength(BioMaxLength)]
        public string? Bio { get; set; }

        public string? Email { get; set; }

        public IFormFile? ProfilePicturePath { get; set; }

        public string? ExistingProfilePicturePath { get; set; }
    }
}

