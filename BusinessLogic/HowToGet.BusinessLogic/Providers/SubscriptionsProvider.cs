using System.Collections.Generic;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Users;
using HowToGet.Repository.Interfaces;

namespace HowToGet.BusinessLogic.Providers
{
	public class SubscriptionsProvider : ISubscriptionsProvider
	{
		private readonly ISubscriptionsRepository _repository;

		public SubscriptionsProvider(ISubscriptionsRepository repository)
		{
			_repository = repository;
		}

		public IEnumerable<LaunchSubscription> GetAllSubscriptions(int omit, int count)
		{
			return _repository.GetAllSubscriptions(omit, count);
		}
	}
}