using System;
using System.Collections.Generic;
using System.Linq;
using HowToGet.CurrencyRates;
using HowToGet.Models.Routes;
using HowToGet.RouteEngine.Interfaces;

namespace HowToGet.RouteEngine.Rankers
{
	public class TotalCostRanker : BasePartialRanker, IPartialRanker
	{
		public PriorityInfo Priorities { get; private set; }

		private readonly ICurrencyRateResolver _resolver;

		public TotalCostRanker(PriorityInfo priorities, ICurrencyRateResolver resolver)
		{
			_resolver = resolver;
			Priorities = priorities;
		}

		public IEnumerable<Route> Sort(IEnumerable<Route> routes, IEnumerable<IPartialRanker> rankers, RouteSortTypes sortType)
		{
			var partialRankers = rankers as IList<IPartialRanker> ?? rankers.ToList();
			var nextRanker = GetNextRanker(partialRankers, Priorities, sortType);

			var result = new List<Route>();

			var res = routes.GroupBy(GetRouteTotalCostInUsd).OrderBy(r => r.Key);

			var res1 = res.Select(r => r.ToList()).ToList();
			foreach (var res2 in res1)
			{
				result.AddRange(nextRanker != null
					? nextRanker.Sort(res2, partialRankers, sortType)
					: res2);
			}
			return result;
		}

		private double GetRouteTotalCostInUsd(Route route)
		{
			double result = 0;

			foreach (var routePart in route.RouteParts)
			{
				if (routePart.Cost > 0 && routePart.CostCurrency > 0)
				{
					var rate = _resolver.GetRateToUsd(routePart.CostCurrency);
					if(rate != null)
						result += routePart.Cost / rate.Rate; // в базе хранятся курсы доллара к валюте, так что тут делим а не умножаем
				}
			}
			if (result == 0.0)
				result = double.MaxValue;
			return result;
		}
	}
}