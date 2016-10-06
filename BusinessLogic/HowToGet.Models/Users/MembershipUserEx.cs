using System;
using System.Web.Security;
using HowToGet.Models.Dictionaries;

namespace HowToGet.Models.Users
{
	public class MembershipUserEx : MembershipUser
	{
		public MembershipUserEx(string pict, GenderTypes gender, string homeCountryId, string homeCityId, string homeCountry, string homeCity, 
								string providerName, string name, object providerUserKey,
		                        string email, string passwordQuestion, string comment, bool isApproved,
		                        bool isLockedOut, DateTime creationDate, DateTime lastLoginDate,
		                        DateTime lastActivityDate, DateTime lastPasswordChangedDate, DateTime lastLockoutDate)
			:base(providerName, name, providerUserKey, email, passwordQuestion, comment, isApproved,
		                        isLockedOut, creationDate, lastLoginDate,
		                        lastActivityDate, lastPasswordChangedDate, lastLockoutDate)
		{
			Picture = pict;
			Gender = gender;
			HomeCountryId = homeCountryId;
			HomeCityId = homeCityId;
			HomeCountry = homeCountry;
			HomeCity = homeCity;
		}

		public string Picture { get; set; }

		public GenderTypes Gender { get; set; }

		public string HomeCountryId { get; set; }
		
		public string HomeCityId { get; set; }

		public string HomeCountry { get; set; }

		public string HomeCity { get; set; }
	}
}