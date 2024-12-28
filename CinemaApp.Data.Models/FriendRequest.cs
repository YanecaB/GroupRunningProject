using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaApp.Data.Models
{
	public class FriendRequest
	{
		[Key]
		public Guid Id { get; set; }

		public Guid SenderId { get; set; }
		[ForeignKey(nameof(SenderId))]
		public ApplicationUser Sender { get; set; } = null!;

		public Guid ReceiverId { get; set; }
		[ForeignKey(nameof(ReceiverId))]
		public ApplicationUser Receiver { get; set; } = null!;

		public DateTime SendRequestDate { get; set; } = DateTime.Now;

		public bool IsDeleted { get; set; } = false;
	}
}
