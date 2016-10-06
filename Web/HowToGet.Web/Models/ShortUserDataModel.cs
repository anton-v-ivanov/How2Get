using HowToGet.Models.Dictionaries;
using HowToGet.Models.Users;
using Resources;

namespace HowToGet.Web.Models
{
	public class ShortUserDataModel
	{
		public string UserId { get; set; }

		public string UserName { get; set; }
		
		public string Picture { get; set; }
		
		public GenderTypes Gender { get; set; }

		public string Country { get; set; }

		public string City { get; set; }

		public ShortUserDataModel()
		{
		}

		public ShortUserDataModel(MembershipUserEx user, string virtualImageFolderName)
		{
			UserId = user.ProviderUserKey.ToString();
			UserName = user.UserName;
			Picture = !string.IsNullOrEmpty(user.Picture) ? string.Format("{0}/{1}", virtualImageFolderName, user.Picture) : Urls.EmptyThumbnail;
			Gender = user.Gender;
			Country = user.HomeCountry;
			City = user.HomeCity;
		}
	}
}