using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Routes;
using HowToGet.RouteEngine.Interfaces;

namespace HowToGet.RouteEngine.Rankers
{
	public class BasePartialRanker
	{
		protected IPartialRanker GetNextRanker(IEnumerable<IPartialRanker> rankers, PriorityInfo currentPriority, RouteSortTypes sortType)
		{
			switch (sortType)
			{
				case RouteSortTypes.Time:
					return rankers
							.Where(s => s.Priorities.SortByTimePriority > currentPriority.SortByTimePriority)
							.OrderBy(s => s.Priorities.SortByTimePriority).FirstOrDefault();
					
				case RouteSortTypes.Price:
					return rankers
							.Where(s => s.Priorities.SortByPricePriority > currentPriority.SortByPricePriority)
							.OrderBy(s => s.Priorities.SortByPricePriority).FirstOrDefault();

				default:
					return rankers
							.Where(s => s.Priorities.DefaultPriority > currentPriority.DefaultPriority)
							.OrderBy(s => s.Priorities.DefaultPriority).FirstOrDefault();
			}
		}
	}
}