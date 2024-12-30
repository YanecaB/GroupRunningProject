using System;
using CinemaApp.Data.Models;
using CinemaApp.Web.ViewModels.Notification;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface INotificationService
	{
		Task GenerateEventNotificationsAsync();

		Task<IEnumerable<NotificationViewModel>> GetNotificationsByUserIdAsync(Guid userId);

		Task GenerateFriendRequestNotificationsAsync(FriendRequest friendRequest);
    }
}

