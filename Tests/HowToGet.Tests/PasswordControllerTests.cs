using System.Reflection;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Hosting.Interfaces;
using HowToGet.IoC;
using HowToGet.Web.API;
using HowToGet.Web.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HowToGet.Tests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class PasswordControllerTests
	{
		public PasswordControllerTests()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext { get; set; }

		#region Additional test attributes

		[TestInitialize]
		public void Init()
		{
			DependencyContainer.RegisterTypes(Assembly.GetExecutingAssembly());
		}
	
		#endregion

		[TestMethod]
		public void ForgotPasswordTest()
		{
			var userProvider = DependencyContainer.ResolveType<IUserProvider>();

			var controller = new PasswordController(userProvider);
			controller.ForgotPassword(new ForgotPasswordModel{Email = "lucs.reality@gmail.com"});
			System.Threading.Thread.Sleep(10000);
		}
	}
}
