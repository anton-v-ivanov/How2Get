using System.Collections.Generic;
using HowToGet.Models.Notifications;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace HowToGet.Repository.Repositories
{
	public class NotificationRepository : INotificationRepository
	{
		private const string CollectionName = "notifications";

		public IEnumerable<Notification> GetNotifications(ObjectId userId)
		{
			var collection = MongoHelper.Database.GetCollection<Notification>(CollectionName);
			var query = Query.And(
							Query.Or(
								Query.EQ("u", userId), 
								Query.EQ("u", ObjectId.Empty)), 
							Query.EQ("r", false));
			return collection.Find(query);
		}

		public void AddNotification(Notification notification)
		{
			var collection = MongoHelper.Database.GetCollection<Notification>(CollectionName);
			collection.Save(notification);
		}

		public void MarkAsRead(ObjectId id)
		{
			var collection = MongoHelper.Database.GetCollection<Notification>(CollectionName);
			var query = Query.EQ("_id", id);
			var update = Update.Set("r", true);
			collection.Update(query, update);
		}
	}
}