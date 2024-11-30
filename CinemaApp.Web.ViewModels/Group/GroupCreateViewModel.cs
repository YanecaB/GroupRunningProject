using System;
using System.ComponentModel.DataAnnotations;
using static CinemaApp.Common.EntityValidationConstants.Group;

namespace CinemaApp.Web.ViewModels.Group
{
	public class GroupCreateViewModel
	{
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

        public DateTime CreatedDate { get; set; }
    }
}

