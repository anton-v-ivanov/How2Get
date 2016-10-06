using HowToGet.BusinessLogic.Events;
using HowToGet.Models.Analytics;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Notifications;

namespace HowToGet.BusinessLogic.Interfaces
{
	public interface IActionEvents
	{
		event ActionEvents.UserActionDelegate UserAction;
		
		event ActionEvents.BonusReceivedDelegate BonusReceived;
		
		event ActionEvents.NotificationCreatedDelegate NotificationCreated;
		
		void OnUserAction(ActionBase action);
		
		void OnBonusReceived(string userId, BonusType bonusType);
		
		void OnNotificationCreated(Notification notification);
	}
}