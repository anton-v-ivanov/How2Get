using System.Linq;
using System.Web.Security;
using HowToGet.Models.Users;

namespace HowToGet.Web.Helpers
{
	public static class MembershipUserCollectionExtensions
	{
		public static MembershipUserEx First(this MembershipUserCollection collection)
		{
			return collection.Cast<MembershipUserEx>().FirstOrDefault();
		}
	}
}