using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CinemaApp.Common.EntityValidationConstants.Event;
using static CinemaApp.Common.EntityValidationMessages.Event;

namespace CinemaApp.Data.Models
{
	public class Event
	{
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = TitleRequired)]
        [MinLength(TitleMinLength, ErrorMessage = TitleMinLengthMessage)]
        [MaxLength(TitleMaxLength, ErrorMessage = TitleMaxLengthMessage)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = DescriptionRequired)]
        [MinLength(DescriptionMinLength, ErrorMessage = DescriptionMinLengthMessage)]
        [MaxLength(DescriptionMaxLength, ErrorMessage = DescriptionMaxLengthMessage)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = DateRequired)]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = LocationRequired)]
        [MinLength(LocationMinLength, ErrorMessage = LocationMinLengthMessage)]
        [MaxLength(LocationMaxLength, ErrorMessage = LocationMaxLengthMessage)]
        public string Location { get; set; } = null!;

        public Guid OrganizerId { get; set; }
        [ForeignKey(nameof(OrganizerId))]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        [Required(ErrorMessage = DistanceRequired)]
        public int Distance { get; set; }

        public Guid GroupId { get; set; }
        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; } = null!;

        public ICollection<ApplicationUserEvent> UsersEvents { get; set; } = new HashSet<ApplicationUserEvent>();

        public bool IsDeleted { get; set; } = false;

        public bool IsPassed { get; set; } = false;
    }
}

