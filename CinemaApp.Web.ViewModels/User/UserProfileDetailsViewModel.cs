using System;
using CinemaApp.Web.ViewModels.Group;

namespace CinemaApp.Web.ViewModels.User
{
	public class UserProfileDetailsViewModel
	{        
        public string Username { get; set; } = null!;

        public string? Bio { get; set; }
        
        public string? ProfilePicturePath { get; set; }

        public bool IsBanned { get; set; }

        public bool SendButtonOrDeleteButton { get; set; } = true;

        public ICollection<GroupIndexViewModel> AdminGroups { get; set; } = null!;
    }
}

