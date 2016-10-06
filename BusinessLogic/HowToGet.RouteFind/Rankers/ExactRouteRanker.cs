using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Routes;
using HowToGet.RouteEngine.Interfaces;

namespace HowToGet.RouteEngine.Rankers
{
	public class ExactRouteRanker : BasePartialRanker, IPartialRanker
	{
		public PriorityInfo Priorities { get; private set; }

		public ExactRouteRanker(PriorityInfo priorities)
		{
			Priorities = priorities;
		}

		public IEnumerable<Route> Sort(IEnumerable<Route> routes, IEnumerable<IPartialRanker> rankers, RouteSortTypes sortType)
		{
			var partialRankers = rankers as IList<IPartialRanker> ?? rankers.ToList();
			var nextRanker = GetNextRanker(partialRankers, Priorities, sortType);

			var result = routes as IList<Route> ?? routes.ToList();
			
			for (int i = 0; i < result.Count() - 1; i++)
			{
				if (string.IsNullOrEmpty(result[i].UserId))
				{
					var tmpRoute = result[i];
					result[i] = result[i + 1];
					result[i + 1] = tmpRoute;
				}
			}
			if (nextRanker != null)
				result = nextRanker.Sort(result, partialRankers, sortType).ToList();
			
			return result;
		}
	}
}