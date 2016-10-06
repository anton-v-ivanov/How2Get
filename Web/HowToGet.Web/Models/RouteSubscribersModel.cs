using System.Collections.Generic;

namespace HowToGet.Web.Models
{
	public class RouteSubscribersModel
	{
		public int Count { get; set; }

		public IEnumerable<ShortUserDataModel> Users { get; set; }
	}
}