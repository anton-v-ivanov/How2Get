using HowToGet.Models.Dictionaries;

namespace HowToGet.Models.Routes
{
	public class ShortRouteInfo
	{
		public string Id { get; set; }
		
		public string UserId { get; set; }

		public string UserName { get; set; }

		public string OriginCityId { get; set; }
		
		public CityShortInfo OriginCityInfo { get; set; }

		public string DestinationCityId { get; set; }

		public CityShortInfo DestinationCityInfo { get; set; }
	}
}