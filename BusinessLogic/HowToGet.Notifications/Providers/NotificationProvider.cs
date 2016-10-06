using System.Collections.Generic;
using HowToGet.Models.Notifications;
using HowToGet.Notifications.Interfaces;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;

namespace HowToGet.Notifications.Providers
{
	public class NotificationProvider : INotificationProvider
	{
		private readonly INotificationRepository _notificationRepository;

		public NotificationProvider(INotificationRepository notificationRepository)
		{
			_notificationRepository = notificationRepository;
		}

		public IEnumerable<Notification> GetNotifications(string userId)
		{
			var objectId = MongoHelper.GetObjectIdFromString(userId, string.Format("{0} is not valid user id", userId));
			return _notificationRepository.GetNotifications(objectId);
		}

		public void AddNotification(Notification notification)
		{
			_notificationRepository.AddNotification(notification);
		}

		public void MarkAsRead(string id)
		{
			var objectId = MongoHelper.GetObjectIdFromString(id, string.Format("{0} is not valid notification id", id));
			_notificationRepository.MarkAsRead(objectId);
		}
	}
}