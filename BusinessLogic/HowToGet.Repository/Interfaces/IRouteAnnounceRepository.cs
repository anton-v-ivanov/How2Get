using System.Collections.Generic;
using HowToGet.Models.Routes;

namespace HowToGet.Repository.Interfaces
{
	public interface IRouteAnnounceRepository
	{
		void CreateSubscription(RouteUserSubscription subscription);
		
		void RemoveSubscription(RouteUserSubscription subscription);
		
		IEnumerable<RouteUserSubscription> GetSubscriptions(string origin, string destination);
		
		RouteUserSubscription FindSubscription(string origin, string destination, string email, string userId);
	}
}