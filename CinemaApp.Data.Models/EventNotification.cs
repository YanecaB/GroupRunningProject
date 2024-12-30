using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CinemaApp.Common.EntityValidationMessages.Notification;

namespace CinemaApp.Data.Models
{
	public class EventNotification : Notification
	{        
        [Required(ErrorMessage = EventIdRequired)]
        public Guid EventId { get; set; }
        [ForeignKey(nameof(EventId))]
        public Event Event { get; set; } = null!;
    }
}

