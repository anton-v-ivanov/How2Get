using System.Collections.Generic;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Routes;

namespace HowToGet.RouteEngine.Interfaces
{
	public interface IRouteSolver
    {
		List<List<RoutePart>> FindRoute(string originCityId, string destinationCityId, ICollection<CarrierTypes> allowedCarrierTypes, int maxResultCount, int maxTransferCount);
    }
}