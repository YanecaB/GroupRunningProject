using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
	public class ApplicationUserEvent
	{
        public Guid ApplicationUserId { get; set; }
        //[ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public Guid EventId { get; set; }
        //[ForeignKey(nameof(EventId))]
        public virtual Event Event { get; set; } = null!;
    }
}

