using System.Collections.Generic;

namespace HowToGet.BusinessLogic.Interfaces
{
	public interface IRouteAnnounceProvider
	{
		bool IsUserSubscribed(string origin, string destination, string userId);

		IEnumerable<string> GetSubscribedUserIds(string origin, string destination, out int count);
		
		void AddSubscription(string origin, string destination, string email, string userId);
		
		void RemoveSubscription(string origin, string destination, string email);
	}
}