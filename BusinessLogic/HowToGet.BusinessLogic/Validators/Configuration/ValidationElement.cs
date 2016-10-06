using System.Configuration;
using HowToGet.Models.Dictionaries;

namespace HowToGet.BusinessLogic.Validators.Configuration
{
	public class ValidationElement : ConfigurationElement
	{
		private const string TypeElement = "carrier-type";
		private const string MinSpeedElement = "min";
		private const string MaxSpeedElement = "max";

		[ConfigurationProperty(TypeElement, IsKey = true, IsRequired = true)]
		public CarrierTypes Type
		{
			get { return (CarrierTypes)base[TypeElement]; }
			set { base[TypeElement] = value; }
		}

		[ConfigurationProperty(MinSpeedElement, IsRequired = true)]
		public int MinSpeed
		{
			get { return (int)base[MinSpeedElement]; }
			set { base[MinSpeedElement] = value; }
		}	 

		[ConfigurationProperty(MaxSpeedElement, IsRequired = true)]
		public int MaxSpeed
		{
			get { return (int)base[MaxSpeedElement]; }
			set { base[MaxSpeedElement] = value; }
		}	 
	}
}