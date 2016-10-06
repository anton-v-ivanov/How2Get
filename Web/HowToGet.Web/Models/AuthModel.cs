using HowToGet.Models.Users;
using HowToGet.Security.Providers;

namespace HowToGet.Web.Models
{
	public class AuthModel
	{
		public string AuthToken { get; set; }

		public AuthModel()
		{
		}

		public AuthModel(MembershipUserEx user)
		{
			AuthToken = TokenProvider.GenerateAuthToken(user);
		} 
	}
}