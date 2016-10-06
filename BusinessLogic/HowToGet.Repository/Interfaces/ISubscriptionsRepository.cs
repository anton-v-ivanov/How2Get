using System.Collections.Generic;
using HowToGet.Models.Users;

namespace HowToGet.Repository.Interfaces
{
	public interface ISubscriptionsRepository
	{
		IEnumerable<LaunchSubscription> GetAllSubscriptions(int omit, int count);
	}
}