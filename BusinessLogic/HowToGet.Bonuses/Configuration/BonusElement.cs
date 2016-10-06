using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using HowToGet.Models.Dictionaries;

namespace HowToGet.Bonuses.Configuration
{
	public class BonusElement : ConfigurationElement
	{
		private const string ParamsElement = "params";
		private const string NameElement = "name";

		[ConfigurationProperty(NameElement, IsKey = true, IsRequired = true)]
		public BonusType Name
		{
			get
			{
				BonusType result;
				return Enum.TryParse(base[NameElement].ToString(), true, out result) 
						? result 
						: BonusType.NotSet;
			}
			set { base[NameElement] = value; }
		}
		
		[ConfigurationProperty(ParamsElement, IsRequired = true)]
		public string Params
		{
			get { return (string) base[ParamsElement]; }
			set { base[ParamsElement] = value; }
		}	 
	}
}