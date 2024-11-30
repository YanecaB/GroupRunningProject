using System;
namespace CinemaApp.Data.Models
{
	public class Membership
	{
        public Guid ApplicationUserId { get; set; }
        //[ForeignKey(nameof(ApplicationUserId))]
        public virtual ApplicationUser ApplicationUser { get; set; } = null!;

        public Guid GroupId { get; set; }
        //[ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; } = null!;

        public DateTime JoinDate { get; set; } = DateTime.Today;
    }
}

