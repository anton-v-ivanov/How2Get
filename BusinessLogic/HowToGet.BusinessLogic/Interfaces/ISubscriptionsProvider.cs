using System.Collections.Generic;
using HowToGet.Models.Users;

namespace HowToGet.BusinessLogic.Interfaces
{
	public interface ISubscriptionsProvider
	{
		IEnumerable<LaunchSubscription> GetAllSubscriptions(int omit, int count);
	}
}