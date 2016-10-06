using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Routes;
using HowToGet.RouteEngine.Interfaces;

namespace HowToGet.RouteEngine
{
	public class RouteRanker: IRouteRanker
	{
		private readonly IEnumerable<IPartialRanker> _partialRankers;
		public RouteRanker(IEnumerable<IPartialRanker> partialRankers)
		{
			_partialRankers = partialRankers;
		}

		public IEnumerable<Route> SortResults(IEnumerable<Route> routes, RouteSortTypes sortType)
		{
			IPartialRanker ranker;
			switch (sortType)
			{
				case RouteSortTypes.Time:
					ranker = _partialRankers.OrderBy(s => s.Priorities.SortByTimePriority).First();
					break;

				case RouteSortTypes.Price:
					ranker = _partialRankers.OrderBy(s => s.Priorities.SortByPricePriority).First();
					break;

				default:
					ranker = _partialRankers.OrderBy(s => s.Priorities.DefaultPriority).First();
					break;
			}
			return ranker.Sort(routes, _partialRankers, sortType);
		}
 
	}
}