using System;
using CinemaApp.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CinemaApp.Common.EntityValidationConstants.Event;

namespace CinemaApp.Data.Seeding.DTOs
{
	public class ImportEventDto
	{
        [Required]
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
        public string Date { get; set; } = null!;

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Location { get; set; } = null!;

        [Required]
        public string OrganizerId { get; set; } = null!;

        [Required]
        public int Distance { get; set; }

        [Required]
        public string GroupId { get; set; } = null!;                     
    }
}

