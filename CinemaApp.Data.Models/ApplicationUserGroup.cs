using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
	public class ApplicationUserGroup
	{
        public Guid ApplicationUserId { get; set; }
        //[ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public Guid GroupId { get; set; }
        //[ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; } = null!;
    }
}

