using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
	public class ConfirmedRequestNotification : Notification
	{
        public Guid NewFriendId { get; set; }
        [ForeignKey(nameof(NewFriendId))]
        public virtual ApplicationUser NewFriend { get; set; } = null!;
    }
}

