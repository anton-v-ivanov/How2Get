using System;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using HowToGet.Analytics;
using HowToGet.Bonuses;
using HowToGet.BusinessLogic.Helpers;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.IoC;
using HowToGet.Web.App_Start;
using Microsoft.AspNet.SignalR;
using NLog;

namespace HowToGet.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
		private static Logger _logger;
		private static Logger CurrentLogger
		{
			get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger()); }
		}

        protected void Application_Start()
        {
			CurrentLogger.Info("ApplicationStart");
	        MvcHandler.DisableMvcResponseHeader = true;
	        try
	        {
				SystemHelper.EnsureMongoIndexes();

				DependencyContainer.RegisterTypes(Assembly.GetExecutingAssembly());

				var webApiresolver = new AutofacWebApiDependencyResolver(DependencyContainer.Container);
				GlobalConfiguration.Configuration.DependencyResolver = webApiresolver;

				var signalrResolver = new AutofacDependencyResolver(DependencyContainer.Container);
				GlobalHost.DependencyResolver = signalrResolver;
		        
				SystemHelper.StartFailedEmailsQueue();

		        var provider = DependencyContainer.ResolveType<ICityProvider>();
		        provider.PreLoad();

				AreaRegistration.RegisterAllAreas();
				WebApiConfig.Register(GlobalConfiguration.Configuration);
				FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
				RouteTable.Routes.MapHubs();
				ViewEngines.Engines.Clear();

		        var eventsConfig = new EventsLinkerConfig(DependencyContainer.ResolveType<IActionEvents>(),
														  DependencyContainer.ResolveType<IAnalyticsActionTracker>(),
														  DependencyContainer.ResolveType<IBonusActionTracker>(),
														  DependencyContainer.ResolveType<IBonusProvider>());
		        eventsConfig.RegisterEvents();
	        }
	        catch (Exception e)
	        {
				CurrentLogger.ErrorException("ApplicationStart exception", e);
		        throw;
	        }
        }
    }
}