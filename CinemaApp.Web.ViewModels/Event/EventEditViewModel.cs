using System;
using System.ComponentModel.DataAnnotations;
using CinemaApp.Web.ViewModels.User;
using static CinemaApp.Common.EntityValidationConstants.Event;

namespace CinemaApp.Web.ViewModels.Event
{
	public class EventEditViewModel
	{
        public string Id { get; set; } = null!;

        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Location { get; set; } = null!;

        public int Distance { get; set; }

        public ICollection<ApplicationUserViewModel> JoinedUsers { get; set; } = new List<ApplicationUserViewModel>();        
    }
}

