using System;
using System.Configuration;
using Autofac;
using HowToGet.BusinessLogic.Configuration;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.BusinessLogic.Providers;
using HowToGet.Hosting.Interfaces;
using HowToGet.Hosting.Providers;

namespace HowToGet.IoC.Modules
{
	public class ProviderModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<CarrierProvider>().As<ICarrierProvider>().SingleInstance();
			builder.RegisterType<CountryProvider>().As<ICountryProvider>().SingleInstance();
			builder.RegisterType<CityProvider>().As<ICityProvider>()
				.WithParameter("searchResultCount", SearchConfig.Instance.CitySearchResultCount)
				.SingleInstance();

			builder.RegisterType<CurrencyProvider>().As<ICurrencyProvider>()
				.WithParameter("euroCurrencyCode", Convert.ToInt32(ConfigurationManager.AppSettings["euro-currency-code"]))
				.WithParameter("usdCurrencyCode", Convert.ToInt32(ConfigurationManager.AppSettings["usd-currency-code"]))
				.SingleInstance();

			builder.RegisterType<RouteProvider>().As<IRouteProvider>()
				.WithParameter("routeCacheTime", SearchConfig.Instance.RouteCacheTimeMinutes)
				.SingleInstance();


			builder.RegisterType<InviteProvider>().As<IInviteProvider>()
				.WithParameter("invitesPerUser", Convert.ToInt32(ConfigurationManager.AppSettings["invites-per-user"]))
				.SingleInstance();
			builder.RegisterType<UserProvider>().As<IUserProvider>().SingleInstance();
			
			builder.RegisterType<FileUploadProvider>().As<IUploadToHostingProvider>().SingleInstance();
			builder.RegisterType<CurrencyRatesProvider>().As<ICurrencyRatesProvider>().SingleInstance();
			builder.RegisterType<SubscriptionsProvider>().As<ISubscriptionsProvider>().SingleInstance();

			base.Load(builder);
		}
	}
}