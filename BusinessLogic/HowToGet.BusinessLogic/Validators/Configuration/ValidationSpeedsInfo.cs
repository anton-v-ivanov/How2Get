using System.Collections.Generic;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Exceptions;

namespace HowToGet.BusinessLogic.Validators.Configuration
{
	public class SpeedInfo
	{
		public int MinSpeed { get; private set; }
		public int MaxSpeed { get; private set; }

		public SpeedInfo(int minSpeed, int maxSpeed)
		{
			MinSpeed = minSpeed;
			MaxSpeed = maxSpeed;
		}
	}

	public class ValidationSpeedsInfo
	{
		private readonly Dictionary<CarrierTypes, SpeedInfo> _speedInfos;

		public ValidationSpeedsInfo(Dictionary<CarrierTypes, SpeedInfo> speedInfos)
		{
			_speedInfos = speedInfos;
		}

		public SpeedInfo GetSpeedInfo(CarrierTypes carrierType)
		{
			var result = _speedInfos[carrierType];
			if(result != null)
				return result;
			
			throw new ValidationException(string.Format("Unable to find validation speed for carrier type: {0}", carrierType));
		}
	}
}