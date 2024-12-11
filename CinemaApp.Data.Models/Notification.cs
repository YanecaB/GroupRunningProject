using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CinemaApp.Common.EntityValidationConstants.Notification;
using static CinemaApp.Common.EntityValidationMessages.Notification;

namespace CinemaApp.Data.Models
{
	public class Notification
	{
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = MessageRequired)]
        [MinLength(MessageMinLength, ErrorMessage = MessageMinLengthMessage)]
        [MaxLength(MessageMaxLength, ErrorMessage = MessageMaxLengthMessage)] 
        public string Message { get; set; } = null!;

        public DateTime Date { get; set; } = DateTime.Now;

        [Required(ErrorMessage = UserIdRequired)]
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required(ErrorMessage = EventIdRequired)]
        public Guid EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; } = null!;
    }
}

