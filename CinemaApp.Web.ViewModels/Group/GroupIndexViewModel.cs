using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaApp.Web.ViewModels.Group
{
	public class GroupIndexViewModel
	{
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string CreatedDate { get; set; } = null!;

    }
}

