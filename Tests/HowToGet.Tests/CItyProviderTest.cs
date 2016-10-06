using System.Diagnostics;
using System.Reflection;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HowToGet.Tests
{
	[TestClass]
	public class CItyProviderTest
	{
		[TestInitialize]
		public void Init()
		{
			DependencyContainer.RegisterTypes(Assembly.GetExecutingAssembly());
		}

		[TestMethod]
		public void GetCityNearTest()
		{
			var provider = DependencyContainer.ResolveType<ICityProvider>();
			const double lat = 45.66019;
			const double lng = 8.79164;
			const string expected = "512e65cd4649091074d0ffdc";
			
			provider.PreLoad();

			var sw = new Stopwatch();
			sw.Start();
			var result = provider.GetCityNear(lat, lng);
			sw.Stop();
			Assert.AreEqual(result.Id, expected);
			Debug.WriteLine("TimeTaken: {0} ms", sw.ElapsedMilliseconds);
		}
	}
}