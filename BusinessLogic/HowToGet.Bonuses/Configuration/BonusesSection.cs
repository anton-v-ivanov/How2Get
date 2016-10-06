using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using HowToGet.Models.Dictionaries;

namespace HowToGet.Bonuses.Configuration
{
	public class BonusesSection : ConfigurationSection
	{
		private const string ParametersElementName = "parameters";

		[ConfigurationProperty(ParametersElementName)]
		public BonusElementCollection BonuseParameters
		{
			get
			{
				return (BonusElementCollection)this[ParametersElementName];
			}
			set
			{
				this[ParametersElementName] = value;
			}
		}

		public static BonusesSection Instance { get; private set; }

		static BonusesSection()
		{
			if(Instance == null)
				Instance = (BonusesSection)ConfigurationManager.GetSection("bonuses");
		}

		public Dictionary<string, string> GetBonusParameters(BonusType bonusType)
		{
			return (BonuseParameters.Cast<BonusElement>()
			                        .Where(bonusElement => bonusElement.Name == bonusType)
			                        .Select(bonusElement => bonusElement.Params.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries))
			                        .Select(
				                        split =>
				                        split.Select(val => val.Split(new[] {'='}, StringSplitOptions.RemoveEmptyEntries))
				                             .Where(splitVal => splitVal.Length > 1)
				                             .ToDictionary(splitVal => splitVal[0].Trim(), splitVal => splitVal[1].Trim())
											)
					).FirstOrDefault();
		}
	}
}