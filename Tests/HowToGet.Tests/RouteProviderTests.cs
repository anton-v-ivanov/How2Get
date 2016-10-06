using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.IoC;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Routes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HowToGet.Tests
{
    [TestClass]
    public class RouteProviderTests
    {
		[TestInitialize]
		public void Init()
		{
			DependencyContainer.RegisterTypes(Assembly.GetExecutingAssembly());
		}

        [TestMethod]
        public void FindRoute()
        {
			var provider = DependencyContainer.ResolveType<IRouteProvider>();
	        const string moscow = "512e65d24649091074d116fa";
	        const string bologna = "512e65c94649091074d0f041";
	        const int maxResultCount = 10;
	        const int maxTransferCount = 3;

			var allowedCarrierTypes = new List<CarrierTypes>
				                          {
					                          CarrierTypes.Airway,
											  CarrierTypes.Bus,
											  CarrierTypes.Car,
											  CarrierTypes.Ferry,
											  CarrierTypes.Railway,
											  CarrierTypes.Taxi
				                          };

	        var result = provider.FindRoute(moscow, bologna, allowedCarrierTypes, RouteSortTypes.NotSet,  maxResultCount, maxTransferCount);
	        foreach (var route in result)
		        Assert.IsTrue(route.RouteParts.All(s => allowedCarrierTypes.Contains(s.CarrierType)));

			allowedCarrierTypes = new List<CarrierTypes>
				                          {
					                          CarrierTypes.Airway,
				                          };

			result = provider.FindRoute(moscow, bologna, allowedCarrierTypes, RouteSortTypes.NotSet, maxResultCount, maxTransferCount);
			foreach (var route in result)
				Assert.IsTrue(route.RouteParts.All(s => allowedCarrierTypes.Contains(s.CarrierType)));

			allowedCarrierTypes = new List<CarrierTypes>
				                          {
					                          CarrierTypes.Railway,
				                          };

			result = provider.FindRoute(moscow, bologna, allowedCarrierTypes, RouteSortTypes.NotSet, maxResultCount, maxTransferCount);
			foreach (var route in result)
				Assert.IsTrue(route.RouteParts.All(s => allowedCarrierTypes.Contains(s.CarrierType)));

        }
    }
}
