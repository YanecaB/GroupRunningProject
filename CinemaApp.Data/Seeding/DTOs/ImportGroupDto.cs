using System;
using System.ComponentModel.DataAnnotations;
using static CinemaApp.Common.EntityValidationConstants.Group;
using static CinemaApp.Common.EntityValidationMessages.Group;

namespace CinemaApp.Data.Seeding.DTOs
{
	public class ImportGroupDto
	{
        [Required]
        public string Id { get; set; } = null!;

        [Required(ErrorMessage = NameRequired)]
        [MinLength(NameMinLength, ErrorMessage = NameMinLengthMessage)]
        [MaxLength(NameMaxLength, ErrorMessage = NameMaxLengthMessage)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = DescriptionRequired)]
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = LocationRequired)]
        [MinLength(LocationMinLength, ErrorMessage = LocationMinLengthMessage)]
        [MaxLength(LocationMaxLength, ErrorMessage = LocationMaxLengthMessage)]
        public string Location { get; set; } = null!;

        [Required]
        public string CreatedDate { get; set; } = null!;

        [Required]
        public string AdminId { get; set; } = null!;    
    }
}

