using System.Reflection;
using Autofac;
using HowToGet.CurrencyRates;
using HowToGet.RouteEngine;
using HowToGet.RouteEngine.Interfaces;
using HowToGet.RouteEngine.Rankers.Configuration;
using HowToGet.RouteEngine.RouteSolvers;
using Module = Autofac.Module;

namespace HowToGet.IoC.Modules
{
	public class RouteEngineModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<CurrencyRateResolver>().As<ICurrencyRateResolver>().SingleInstance();

			builder.RegisterType<RouteEngine.RouteEngine>().As<IRouteEngine>().SingleInstance();
			builder.RegisterType<CustomRouteSolver>().As<IRouteSolver>().SingleInstance();
			builder.RegisterType<RouteRanker>().As<IRouteRanker>().SingleInstance();
			builder.RegisterAssemblyTypes(Assembly.Load("HowToGet.RouteEngine"))
				.AssignableTo<IPartialRanker>()
				.As<IPartialRanker>()
				.WithParameter(
					(pinfo, ctx) =>
					{
						return
						  pinfo.Name == "priorities" &&
						  pinfo.ParameterType == typeof(PriorityInfo);
					},

					(pinfo, ctx) =>
					{
						return PartialRankersSection.Instance.GetRankerPriority(((pinfo.Member).DeclaringType).Name);
					}
				)
				.SingleInstance();

			base.Load(builder);
		}
	}
}