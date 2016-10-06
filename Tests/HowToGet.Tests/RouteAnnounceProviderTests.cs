using System.Linq;
using System.Reflection;
using System.Threading;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HowToGet.Tests
{
	[TestClass]
	public class RouteAnnounceProviderTests
	{
		[TestInitialize]
		public void Init()
		{
			DependencyContainer.RegisterTypes(Assembly.GetExecutingAssembly());
		}

		[TestMethod]
		public void AddSubscriptionTest()
		{
			var sut = DependencyContainer.ResolveType<IRouteAnnounceProvider>();
			const string origin = "512e65c94649091074d0f041";
			const string destination = "512e65d64649091074d12414";
			const string email = "lucs.reality@gmail.com";
			
			sut.AddSubscription(origin, destination, email, null);
			Thread.Sleep(20000);
		}

		[TestMethod]
		public void AddSubscriptionWithUserTest()
		{
			var sut = DependencyContainer.ResolveType<IRouteAnnounceProvider>();
			const string origin = "512e65c94649091074d0f041";
			const string destination = "512e65d64649091074d12414";
			const string email = "tonage@me.com";
			const string userId = "528130024649092d605b2712";
			
			sut.AddSubscription(origin, destination, email, userId);
		}

		[TestMethod]
		public void RemoveSubscriptionTest()
		{
			var sut = DependencyContainer.ResolveType<IRouteAnnounceProvider>();
			const string origin = "512e65c94649091074d0f041";
			const string destination = "512e65d64649091074d12414";
			const string email = "lucs.reality@gmail.com";
			
			sut.RemoveSubscription(origin, destination, email);
		}

		[TestMethod]
		public void IsUserSubscribedTest()
		{
			var sut = DependencyContainer.ResolveType<IRouteAnnounceProvider>();
			const string origin = "512e65c94649091074d0f041";
			const string destination = "512e65d64649091074d12414";
			const string userId = "528130024649092d605b2712";

			var result = sut.IsUserSubscribed(origin, destination, userId);
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void GetSubscribedUserIdsTest()
		{
			var sut = DependencyContainer.ResolveType<IRouteAnnounceProvider>();
			const string origin = "512e65c94649091074d0f041";
			const string destination = "512e65d64649091074d12414";
			int count;

			var result = sut.GetSubscribedUserIds(origin, destination, out count);
			Assert.IsTrue(result.Any());
		}
	}
}