using System;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.BusinessLogic.Validators.Configuration;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Routes;
using HowToGet.RouteEngine;

namespace HowToGet.BusinessLogic.Validators
{
	public class RouteValidator : IRouteValidator
	{
		private readonly ICityProvider _cityProvider;
		private readonly ICityValidator _cityValidator;
		private readonly ICurrencyProvider _currencyProvider;
		private readonly bool _checkTime;
		private readonly bool _checkCurrency;
		private readonly ValidationSpeedsInfo _speedsInfo;

		public RouteValidator(ICityProvider cityProvider, ICityValidator cityValidator, ICurrencyProvider currencyProvider, bool checkTime, bool checkCurrency, ValidationSpeedsInfo speedsInfo)
		{
			_cityProvider = cityProvider;
			_cityValidator = cityValidator;
			_currencyProvider = currencyProvider;
			_checkTime = checkTime;
			_checkCurrency = checkCurrency;
			_speedsInfo = speedsInfo;
		}

		public void ValidateRoute(Route route)
		{
			if (route.RouteParts.Count == 0)
				throw new ValidationException("Route doesn't contains route parts");
			
			foreach (var routePart in route.RouteParts)
			{
				_cityValidator.ValidateCity(CityValidator.ValidateCityType.Origin, routePart.OriginCityId);
				_cityValidator.ValidateCity(CityValidator.ValidateCityType.Destination, routePart.DestinationCityId);

				if (_checkTime)
				{
					if (routePart.Time <= 0)
						throw new ValidationException("Time is not specified");

					var speedInfo = _speedsInfo.GetSpeedInfo(routePart.CarrierType);

					var origin = _cityProvider.GetById(routePart.OriginCityId);
					var destination = _cityProvider.GetById(routePart.DestinationCityId);

					var distance = DistanceCalculator.GetDistance(origin.Latitude, origin.Longitude,
					                                              destination.Latitude, destination.Longitude);

					var minTime = (distance/speedInfo.MaxSpeed)*60;
					if (minTime > routePart.Time)
						throw new ValidationException(string.Format("MinTimeExceed:{0};RoutePartId:{1}", minTime, routePart.Id));

					var maxTime = (distance/speedInfo.MinSpeed)*60;
					if (maxTime < routePart.Time)
						throw new ValidationException(string.Format("MaxTimeExceed:{0};RoutePartId:{1}", minTime, routePart.Id));

				}

				if (_checkCurrency)
				{
					if (routePart.Cost <= 0)
						throw new ValidationException("Cost is not specified");

					if (routePart.CostCurrency == 0)
						throw new ValidationException("CostCurrency is not specified");

					if (_currencyProvider.GetById(routePart.CostCurrency) == null)
						throw new ValidationException("Invalid CostCurrency value");
				}
			}
		}
	}
}