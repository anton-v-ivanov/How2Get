using Autofac;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.BusinessLogic.Providers;
using HowToGet.Repository.Interfaces;
using HowToGet.Repository.Repositories;

namespace HowToGet.IoC.Modules
{
	public class OneTimeTokenModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<OneTimeTokenRepository>().As<IOneTimeTokenRepository>().SingleInstance();
			builder.RegisterType<OneTimeTokenProvider>().As<IOneTimeTokenProvider>().SingleInstance();

			base.Load(builder);
		}
 
	}
}