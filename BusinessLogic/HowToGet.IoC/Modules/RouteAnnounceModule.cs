using Autofac;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.BusinessLogic.Providers;
using HowToGet.Repository.Interfaces;
using HowToGet.Repository.Repositories;

namespace HowToGet.IoC.Modules
{
	public class RouteAnnounceModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<RouteAnnounceRepository>().As<IRouteAnnounceRepository>().SingleInstance();
			builder.RegisterType<RouteAnnounceProvider>().As<IRouteAnnounceProvider>().SingleInstance();

			base.Load(builder);
		}
	}
}