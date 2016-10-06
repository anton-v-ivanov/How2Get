using System.Collections.Generic;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Users;

namespace HowToGet.BusinessLogic.Interfaces
{
	public interface IUserProvider
	{
		void ForgotPassword(MembershipUserEx user);

		string GetExternalUserId(string externalId, ExternalAuthServices authService);

		string GeneratePassword();

		void AssociateExternalUser(MembershipUserEx user, string externalId, ExternalAuthServices authService, string accessToken);
		
		void UpdateUserPicture(string userId, string fileName);

		User GetUserById(string userId);
		
		void SendUserCreatedNotification(MembershipUserEx user);
		
		void SendUserCreatedWithPasswordNotification(MembershipUserEx user, string password);

		void ProcessExternalUserCreated(MembershipUserEx user, string password, ExternalAuthServices service);
		
		IEnumerable<MembershipUserEx> GetTopUsers();
		
		void FillAddressData(MembershipUserEx user);

		void UpdateUserName(string userId, string userName);
		
		string GetLastLocation(string userId);
		
		void SaveLastLocation(string userId, string cityId);
	}
}