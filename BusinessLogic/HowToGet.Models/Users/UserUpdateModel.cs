using HowToGet.Models.Dictionaries;

namespace HowToGet.Models.Users
{
	public class UserUpdateModel
	{
		public string UserName { get; set; }
		
		public GenderTypes Gender { get; set; }

		public string CountryId { get; set; }

		public string CityId { get; set; }

		public string Email { get; set; }
	}
}