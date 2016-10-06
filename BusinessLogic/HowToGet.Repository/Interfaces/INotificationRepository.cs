using System.Collections.Generic;
using HowToGet.Models.Notifications;
using MongoDB.Bson;

namespace HowToGet.Repository.Interfaces
{
	public interface INotificationRepository
	{
		IEnumerable<Notification> GetNotifications(ObjectId userId);

		void AddNotification(Notification notification);

		void MarkAsRead(ObjectId id);
	}
}