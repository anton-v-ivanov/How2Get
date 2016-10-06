using System.Configuration;
using System.Linq;

namespace HowToGet.BusinessLogic.Validators.Configuration
{
	public class ValidationSection : ConfigurationSection
	{
		private const string SpeedElementName = "speed";
		private const string CheckTimeElementName = "check-time";
		private const string CheckCurrencyElementName = "check-currency";

		[ConfigurationProperty(SpeedElementName, IsRequired = true)]
		private ValidationElementCollection Speeds
		{
			get
			{
				return (ValidationElementCollection)this[SpeedElementName];
			}
		}

		[ConfigurationProperty(CheckTimeElementName, IsRequired = true)]
		public bool CheckTime
		{
			get
			{
				return (bool)(this[CheckTimeElementName]);
			}
		}

		[ConfigurationProperty(CheckCurrencyElementName, IsRequired = true)]
		public bool CheckCurrency
		{
			get
			{
				return (bool)(this[CheckCurrencyElementName]);
			}
		}

		public static ValidationSection Instance { get; private set; }

		static ValidationSection()
		{
			if (Instance == null)
				Instance = (ValidationSection)ConfigurationManager.GetSection("validation");

		}



		public ValidationSpeedsInfo GetSpeedsInfo()
		{
			var speeds = Speeds
							.Cast<ValidationElement>()
							.ToDictionary(s => s.Type, s => new SpeedInfo(s.MinSpeed, s.MaxSpeed));
			return new ValidationSpeedsInfo(speeds);
		}
	}
}