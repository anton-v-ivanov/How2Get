using System.Collections.Generic;
using HowToGet.Models.Notifications;

namespace HowToGet.Notifications.Interfaces
{
	public interface INotificationProvider
	{
		IEnumerable<Notification> GetNotifications(string userId);

		void AddNotification(Notification notification);

		void MarkAsRead(string id);
	}
}