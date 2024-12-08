using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CinemaApp.Common.EntityValidationConstants.Notification;

namespace CinemaApp.Data.Models
{
	public class Notification
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
        [MinLength(MessageMinLength)]
        [MaxLength(MessageMaxLength)]
        public string Message { get; set; } = null!;

		public DateTime Date { get; set; } = DateTime.Now;

		public Guid UserId { get; set; }
		[ForeignKey(nameof(UserId))]
		public ApplicationUser User { get; set; } = null!;

		public Guid EventId { get; set; }
		[ForeignKey(nameof(EventId))]
		public Event Event { get; set; } = null!;
	}
}

