using System;
namespace CinemaApp.Web.ViewModels.User
{
	public class ApplicationUserViewModel
	{
		public string Id { get; set; } = null!;

		public string? UserName { get; set; }

		public string? Email { get; set; }
	}
}

