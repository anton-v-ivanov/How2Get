using System;
using Autofac;
using HowToGet.Bonuses;
using HowToGet.Bonuses.BonusDefinitions;
using HowToGet.Bonuses.Configuration;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Interfaces;
using HowToGet.Repository.Repositories;
using Module = Autofac.Module;

namespace HowToGet.IoC.Modules
{
	public class BonusModule: Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<BonusRepository>().As<IBonusRepository>().SingleInstance();
			
			builder.RegisterType<BonusProvider>().As<IBonusProvider>().SingleInstance();

			var parameters = BonusesSection.Instance.GetBonusParameters(BonusType.Registration);
			builder.RegisterType<RegistrationBonus>().As<IBonus>()
				.WithParameter("usersLimit", Convert.ToInt32(parameters["usersLimit"]))
				.SingleInstance();
			
			builder.RegisterType<BonusActionTracker>().As<IBonusActionTracker>().SingleInstance();

			base.Load(builder);
		}
	}
}