using System;
using CinemaApp.Web.ViewModels.Notification;

namespace CinemaApp.Services.Data.Interfaces
{
	public interface INotificationService
	{
		Task GenerateNotificationsAsync();

		Task<IEnumerable<NotificationViewModel>> GetNotificationsByUserIdAsync(Guid userId);
    }
}

