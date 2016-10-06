using System.Collections.Generic;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Bonuses;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Notifications;
using HowToGet.Notifications.Interfaces;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;

namespace HowToGet.Bonuses
{
	public class BonusProvider : IBonusProvider
	{
		private readonly IBonusRepository _bonusRepository;
		private readonly IActionEvents _actionEvents;
		private readonly INotificationProvider _notificationProvider;

		public BonusProvider(IBonusRepository bonusRepository, IActionEvents actionEvents, INotificationProvider notificationProvider)
		{
			_bonusRepository = bonusRepository;
			_actionEvents = actionEvents;
			_notificationProvider = notificationProvider;
		}

		public void OnBonusReceived(string userId, BonusType bonusType)
		{
			var bonus = new Bonus(userId, bonusType);
			_bonusRepository.AddBonus(bonus);

			var notification = new Notification(userId, NotificationType.Bonus, bonusType);
			_notificationProvider.AddNotification(notification);

			_actionEvents.OnNotificationCreated(notification);
		}

		public IEnumerable<Bonus> GetBonuses(string userId)
		{
			var objectId = MongoHelper.GetObjectIdFromString(userId, string.Format("{0} is not valid user id", userId));
			return _bonusRepository.GetBonuses(objectId);
		}
	}
}