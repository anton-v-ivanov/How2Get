using System.Collections.Generic;
using HowToGet.Models.Notifications;
using HowToGet.Models.Users;

namespace HowToGet.Notifications.Interfaces
{
	public interface IEmailNotificationProvider
	{
		void Send(EmailNotificationType emailNotificationType, string initiatorId, Dictionary<string, object> parameters);
		
		void Send(EmailNotificationType emailNotificationType, User initiator, Dictionary<string, object> parameters);
		
		void SendUnregistered(EmailNotificationType emailNotificationType, string email, Dictionary<string, object> parameters);
	}
}