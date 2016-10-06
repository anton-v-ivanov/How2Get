using System;
using System.Configuration;

namespace HowToGet.RouteEngine.Rankers.Configuration
{
	public class PartialRankersSection : ConfigurationSection
	{
		private const string PriorityElementName = "priority";

		[ConfigurationProperty(PriorityElementName)]
		public PriorityElementCollection PartialRankersPriorities
		{
			get
			{
				return (PriorityElementCollection)this[PriorityElementName];
			}
			set
			{
				this[PriorityElementName] = value;
			}
		}

		public static PartialRankersSection Instance { get; private set; }

		static PartialRankersSection()
		{
			if(Instance == null)
				Instance = (PartialRankersSection)ConfigurationManager.GetSection("partialRankers");
		}

		public PriorityInfo GetRankerPriority(string rankerName)
		{
			foreach (PriorityElement partialRankersPriority in PartialRankersPriorities)
			{
				if (partialRankersPriority.Name.Equals(rankerName, StringComparison.InvariantCultureIgnoreCase))
					return new PriorityInfo
						       {
								   SortByTimePriority = partialRankersPriority.SortByTimePriority,
								   SortByPricePriority = partialRankersPriority.SortByPricePriority,
								   DefaultPriority = partialRankersPriority.DefaultPriority
						       };
			}
			return null;
		}
	}
}