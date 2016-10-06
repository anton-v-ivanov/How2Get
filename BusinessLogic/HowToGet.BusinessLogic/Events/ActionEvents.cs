using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Analytics;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Notifications;

namespace HowToGet.BusinessLogic.Events
{
	public class ActionEvents: IActionEvents
	{
		public delegate void UserActionDelegate(ActionBase action);
		public event UserActionDelegate UserAction;
		
		public delegate void BonusReceivedDelegate(string userId, BonusType bonusType);
		public event BonusReceivedDelegate BonusReceived;
		
		public delegate void NotificationCreatedDelegate(Notification notification);
		public event NotificationCreatedDelegate NotificationCreated;

		public void OnBonusReceived(string userId, BonusType bonusType)
		{
			if (BonusReceived != null) BonusReceived(userId, bonusType);
		}

		public void OnUserAction(ActionBase action)
		{
			if (UserAction != null) UserAction(action);
		}

		public void OnNotificationCreated(Notification notification)
		{
			if (NotificationCreated != null) NotificationCreated(notification);
		}
	}
}