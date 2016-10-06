using System;
using System.Collections.Generic;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Users;
using MongoDB.Bson;

namespace HowToGet.Repository.Interfaces
{
	public interface IUserRepository
	{
		string GetUserIdByExternalId(string externalId, ExternalAuthServices authService);

		void AssociateExternalUser(ExternalUserLink userLink);

		void UpdateUserData(User user);

		void DeleteUser(ObjectId userId);

		List<User> GetAllUsers(int pageIndex, int pageSize, out int totalRecords);

		long GetNumberOfUsers();
		
		int GetNumberOfUsersOnline(DateTime compareTime);
		
		User GetUser(string email, bool userIsOnline);

		User GetUser(ObjectId objectId, bool userIsOnline);

		string GetUserNameByEmail(string email);

		User GetUserByEmail(string email);
		
		void UpdateUserPicture(ObjectId userId, string fileName);
		
		void UpdateUserName(ObjectId userId, string userName);

		IEnumerable<User> GetUsersByName(string username);

		void UpdateUserRoles(ObjectId[] userIds, List<string> roles);
		
		IEnumerable<User> GetUsersInRole(string role);
		
		IEnumerable<User> FindUsersInRole(string role, ObjectId userId);
		
		void RemoveUsersFromRoles(IEnumerable<ObjectId> usersIds, IEnumerable<string> roles);
		
		UserLocation GetLastLocation(string userId);

		void SaveLastLocation(UserLocation location);
	}
}