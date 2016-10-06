using Autofac;
using HowToGet.Analytics;
using HowToGet.BusinessLogic.Events;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Repository.Interfaces;
using HowToGet.Repository.Repositories;

namespace HowToGet.IoC.Modules
{
	public class ActionModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ActionTrackerRepository>().As<IActionTrackerRepository>().SingleInstance();
			builder.RegisterType<AnalyticsActionTracker>().As<IAnalyticsActionTracker>().SingleInstance();
			builder.RegisterType<ActionEvents>().As<IActionEvents>().SingleInstance();

			base.Load(builder);
		}
	}
}