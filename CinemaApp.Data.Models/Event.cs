using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CinemaApp.Common.EntityValidationConstants.Event;

namespace CinemaApp.Data.Models
{
	public class Event
	{
		[Key]
		public Guid Id { get; set; }

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

		public Guid OrganizerId { get; set; }
		[ForeignKey(nameof(OrganizerId))]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

		[Required]
		public int Distance { get; set; }

		public Guid GroupId { get; set; }
		[ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; } = null!;

		public ICollection<ApplicationUserEvent> UsersEvents { get; set; } = new HashSet<ApplicationUserEvent>();

		public bool IsDeleted { get; set; } = false;
	}
}

