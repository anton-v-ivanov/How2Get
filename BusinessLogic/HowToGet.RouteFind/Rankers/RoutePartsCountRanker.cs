using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Routes;
using HowToGet.RouteEngine.Interfaces;

namespace HowToGet.RouteEngine.Rankers
{
	public class RoutePartsCountRanker : BasePartialRanker, IPartialRanker
	{
		public PriorityInfo Priorities { get; private set; }

		public RoutePartsCountRanker(PriorityInfo priorities)
		{
			Priorities = priorities;
		}

		public IEnumerable<Route> Sort(IEnumerable<Route> routes, IEnumerable<IPartialRanker> rankers, RouteSortTypes sortType)
		{
			var partialRankers = rankers as IList<IPartialRanker> ?? rankers.ToList();
			var nextRanker = GetNextRanker(partialRankers, Priorities, sortType);

			var result = new List<Route>();

			var res = routes.GroupBy(r => r.RouteParts.Count).OrderBy(r => r.Key);

			var res1 = res.Select(r => r.ToList()).ToList();
			foreach (var res2 in res1)
			{
				result.AddRange(nextRanker != null
					? nextRanker.Sort(res2, partialRankers, sortType)
					: res2);
			}
			
			return result;
		}
	}
}