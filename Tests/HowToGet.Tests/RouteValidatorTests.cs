using System.Collections.Generic;
using System.Reflection;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.IoC;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Routes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HowToGet.Tests
{
	[TestClass]
	public class RouteValidatorTests
	{
		[TestInitialize]
		public void Init()
		{
			DependencyContainer.RegisterTypes(Assembly.GetExecutingAssembly());
		}

		[TestMethod]
		public void ValidateRouteTest()
		{
			var route = new Route
				            {
					            RouteParts = new List<RoutePart>
						                         {
							                         new RoutePart
								                         {
															 OriginCityId = "512e65c94649091074d0f041",
															 DestinationCityId = "512e65d64649091074d12414",
															 Time = 10,
															 CarrierType = CarrierTypes.Airway
								                         }
						                         }
				            };
			var validator = DependencyContainer.ResolveType<IRouteValidator>();
			validator.ValidateRoute(route);
			
			route.RouteParts[0].CarrierType = CarrierTypes.Railway;
			route.RouteParts[0].Time = 10000;
			validator.ValidateRoute(route);
		}
	}
}