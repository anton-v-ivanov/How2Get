using System.Text.RegularExpressions;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Users;
using Resources;

namespace HowToGet.Web.Models
{
	public class FullUserDataModel
	{
		public string UserId { get; set; }

		public string UserName { get; set; }

		public string Picture { get; set; }
		
		public GenderTypes Gender { get; set; }

		public string Country { get; set; }

		public string City { get; set; }

		public string Email { get; set; }

		public FullUserDataModel()
		{
			
		}

		public FullUserDataModel(MembershipUserEx user, string virtualImageFolderName)
		{
			UserId = user.ProviderUserKey.ToString();
			UserName = user.UserName;
			Picture = !string.IsNullOrEmpty(user.Picture) ? string.Format("{0}/{1}", virtualImageFolderName, user.Picture) : Urls.EmptyThumbnail;
			Gender = user.Gender;
			Country = user.HomeCountry;
			City = user.HomeCity;
			Email = ObfuscateEmail(user.Email);
		}

		private static string ObfuscateEmail(string email)
		{
			var strs = email.Split(new[]{'@'});
			if (strs.Length < 2)
			{
				return email;
			}
			
			string result = string.Empty;
			for (int i = 0; i < strs[0].Length; i++)
			{
				var ch = email[i];
				if (i < 2)
					result += ch;
				else
					result += "•";
			}
			result += "@" + strs[1];
			return result;
		}
	}
}