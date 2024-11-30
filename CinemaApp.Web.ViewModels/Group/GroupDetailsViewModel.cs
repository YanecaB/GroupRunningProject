using System;
namespace CinemaApp.Web.ViewModels.Group
{
	public class GroupDetailsViewModel
	{
        public string Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string CreatedDate { get; set; } = null!;

        public int MembersCount { get; set; }
    }
}

