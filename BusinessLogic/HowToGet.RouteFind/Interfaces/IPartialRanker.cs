using System.Collections.Generic;
using HowToGet.Models.Routes;

namespace HowToGet.RouteEngine.Interfaces
{
	public interface IPartialRanker
	{
		PriorityInfo Priorities { get; }

		IEnumerable<Route> Sort(IEnumerable<Route> routes, IEnumerable<IPartialRanker> rankers, RouteSortTypes sortType);
	}
}