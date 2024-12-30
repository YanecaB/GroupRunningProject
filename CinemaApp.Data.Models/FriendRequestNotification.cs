using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
	public class FriendRequestNotification : Notification
	{		
		public Guid FriendRequestId { get; set; }
		[ForeignKey(nameof(FriendRequestId))]
		public FriendRequest FriendRequest { get; set; } = null!;
	}
}

