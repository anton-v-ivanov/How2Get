using Autofac;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.BusinessLogic.Validators;
using HowToGet.BusinessLogic.Validators.Configuration;

namespace HowToGet.IoC.Modules
{
	public class ValidatorModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<CityValidator>().As<ICityValidator>().SingleInstance();
			builder.RegisterType<RouteValidator>().As<IRouteValidator>()
				.WithParameter("checkTime", ValidationSection.Instance.CheckTime)
				.WithParameter("checkCurrency", ValidationSection.Instance.CheckCurrency)
				.WithParameter("speedsInfo", ValidationSection.Instance.GetSpeedsInfo())
				.SingleInstance();
			builder.RegisterType<CountryValidator>().As<ICountryValidator>().SingleInstance();

			base.Load(builder);
		}
	}
}