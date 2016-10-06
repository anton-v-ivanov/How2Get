using System.Net;
using System.Threading;
using LaunchPage.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LaunchPage.Tests
{
	[TestClass]
	public class SubscribeTests
    {
		[TestMethod]
		public void Subscribe()
		{
			var controller = new SubscribeController();
			const string email = "anton@how2get.to";
			
			var result = controller.Subscribe(new SubscribeModel{Email = email, Referrer = "test.local"});
			Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);

			result = controller.Subscribe(new SubscribeModel { Email = email, Referrer = "test.local" });
			Assert.IsTrue(result.StatusCode == HttpStatusCode.Conflict);

			Thread.Sleep(20000);
		}
    }
}
