using System.Configuration;
using Autofac;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.BusinessLogic.Providers;
using HowToGet.Repository.Interfaces;
using HowToGet.Repository.Repositories;

namespace HowToGet.IoC.Modules
{
	public class FoursqModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<FoursqRepository>().As<IFoursqRepository>().SingleInstance();
			builder.RegisterType<FoursqProvider>().As<IFoursqProvider>()
				.WithParameter("foursqPushSecret", ConfigurationManager.AppSettings["foursq-push-secret"])
				.SingleInstance();

			base.Load(builder);
		}
	}
}