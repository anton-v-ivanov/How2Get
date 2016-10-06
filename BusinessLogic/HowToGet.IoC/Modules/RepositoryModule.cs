using Autofac;
using HowToGet.Repository.Interfaces;
using HowToGet.Repository.Repositories;

namespace HowToGet.IoC.Modules
{
	public class RepositoryModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<CarrierRepository>().As<ICarrierRepository>().SingleInstance();
			builder.RegisterType<CityRepository>().As<ICityRepository>().SingleInstance();
			builder.RegisterType<CountryRepository>().As<ICountryRepository>().SingleInstance();
			builder.RegisterType<CurrencyRepository>().As<ICurrencyRepository>().SingleInstance();
			builder.RegisterType<FailedEmailsRepository>().As<IFailedEmailsRepository>().SingleInstance();
			builder.RegisterType<RouteRepository>().As<IRouteRepository>().SingleInstance();
			builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
			builder.RegisterType<CurrencyRatesRepository>().As<ICurrencyRatesRepository>().SingleInstance();
			builder.RegisterType<SubscriptionsRepository>().As<ISubscriptionsRepository>().SingleInstance();
			builder.RegisterType<InviteRepository>().As<IInviteRepository>().SingleInstance();
			
			base.Load(builder);
		}
	}
}