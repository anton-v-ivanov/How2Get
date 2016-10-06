using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using NLog;

namespace LaunchPage
{
	public class Global : HttpApplication
	{
		private static Logger _logger;
		private static Logger CurrentLogger
		{
			get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger()); }
		}

		protected void Application_Start(object sender, EventArgs e)
		{
			CurrentLogger.Info("Application start");
			
			var indexes = new Dictionary<string, string[]>
				              {
					              {"subscriptions", new[]{"lemail"}},
				              };

			MongoHelper.EnsureIndexes(indexes);
			RegisterWebApi(GlobalConfiguration.Configuration);
		}

		private void RegisterWebApi(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute
				(
					"api",
					"api/{controller}/{action}/{id}",
					new { id = RouteParameter.Optional, action = RouteParameter.Optional }
				);
		}

		protected void Application_End(object sender, EventArgs e)
		{
			CurrentLogger.Info("Application end");
		}
	}
}