using System;
using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Routes;
using HowToGet.Repository.Interfaces;
using HowToGet.RouteEngine.Interfaces;

namespace HowToGet.RouteEngine.RouteSolvers
{
    public class CustomRouteSolver: IRouteSolver
    {
	    private readonly IRouteRepository _routeRepository;
	    private readonly ICarrierRepository _carrierRepository;
	    private List<Carrier> _allCarriers;

	    public CustomRouteSolver(IRouteRepository routeRepository, ICarrierRepository carrierRepository)
	    {
		    _routeRepository = routeRepository;
		    _carrierRepository = carrierRepository;
	    }

		public List<List<RoutePart>> FindRoute(string originCityId, string destinationCityId, ICollection<CarrierTypes> allowedCarrierTypes, int maxResultCount, int maxTransferCount)
		{
			_allCarriers = _carrierRepository.GetAll().ToList();

			var result = new List<List<RoutePart>>();
			
			var routesFromOrigin = _routeRepository.GetRoutePartsForOrigin(originCityId);
			foreach (var route in routesFromOrigin)
			{
				if (result.Count > maxResultCount)
					break;
				result.AddRange(Process(destinationCityId, route, allowedCarrierTypes, maxTransferCount, route));
			}

			return result;
		}

		private IEnumerable<List<RoutePart>> Process(string destinationCityId, RoutePart parentRoute, ICollection<CarrierTypes> allowedCarrierTypes, int maxTransferCount, params RoutePart[] routeHistory)
        {
            var result = new List<List<RoutePart>>();
            
			// пересадок больше, чем разрешено
            if(routeHistory.Length > maxTransferCount)
                return result;

			// поиск достиг пункта назначения
            if (parentRoute.DestinationCityId == destinationCityId 
				&& CheckCarrierType(parentRoute.CarrierId, allowedCarrierTypes))
            {
                result.Add(new List<RoutePart> { parentRoute });
                return result;
            }

	        var iteration = _routeRepository.GetRoutePartsForOrigin(parentRoute.DestinationCityId);
            foreach (var path in iteration)
            {
				// провеяем тип передвижения
	            if (!CheckCarrierType(path.CarrierId, allowedCarrierTypes))
		            continue;

				// в итерации достигли пункта назначения
                if (path.DestinationCityId == destinationCityId)
                {
	                var temp = new List<RoutePart>();
	                temp.AddRange(routeHistory);
	                temp.Add(path);
	                result.Add(temp);
                }
                else
                {
					// не достигли пункта назначения, продолжаем перебор из этой точки
                    var tempHistory = routeHistory.ToList();
                    tempHistory.Add(path);
                    result.AddRange(Process(destinationCityId, path, allowedCarrierTypes, maxTransferCount, tempHistory.ToArray()));
                }
            }
            return result;
        }

		private bool CheckCarrierType(string carrierId, ICollection<CarrierTypes> allowedCarrierTypes)
		{
			if (!string.IsNullOrEmpty(carrierId))
			{
				var carrier = _allCarriers.FirstOrDefault(s => s.Id.Equals(carrierId, StringComparison.InvariantCultureIgnoreCase));
				if (carrier != null && allowedCarrierTypes.Contains(carrier.Type))
					return true;
			}
			return false;
		}
    }
}