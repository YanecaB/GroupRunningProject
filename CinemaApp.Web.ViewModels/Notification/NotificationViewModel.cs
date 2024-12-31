using System;
namespace CinemaApp.Web.ViewModels.Notification
{
	public class NotificationViewModel
	{
        public string Id { get; set; } = null!;

        public string Message { get; set; } = null!;

        public string Date { get; set; } = null!;

        public string? EventName { get; set; }

        public string? EventId { get; set; }

        public string? SenderUserName { get; set; }

        public string? NewFriendUsername { get; set; }
    }
}

