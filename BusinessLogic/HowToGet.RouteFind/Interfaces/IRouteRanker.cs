using System.Collections.Generic;
using HowToGet.Models.Routes;

namespace HowToGet.RouteEngine.Interfaces
{
	public interface IRouteRanker
	{
		IEnumerable<Route> SortResults(IEnumerable<Route> routes, RouteSortTypes sortType);
	}
}