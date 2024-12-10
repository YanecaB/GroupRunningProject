using System;
using System.ComponentModel.DataAnnotations;
using static CinemaApp.Common.EntityValidationConstants.Group;

namespace CinemaApp.Data.Seeding.DTOs
{
	public class ImportGroupDto
	{
        [Required]
        public string Id { get; set; } = null!;

        [Required]
        [MinLength(NameMinLength)]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(DescriptionMinLength)]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [MinLength(LocationMinLength)]
        [MaxLength(LocationMaxLength)]
        public string Location { get; set; } = null!;

        [Required]
        public string CreatedDate { get; set; } = null!;

        [Required]
        public string AdminId { get; set; } = null!;    
    }
}

