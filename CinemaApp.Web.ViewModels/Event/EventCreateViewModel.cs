using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CinemaApp.Common.EntityValidationConstants.Event;

namespace CinemaApp.Web.ViewModels.Event
{
	public class EventCreateViewModel
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
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Location { get; set; } = null!;

        public int Distance { get; set; }

        public string GroupId { get; set; } = null!;
    }
}

