using System;
using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Dictionaries;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Users;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace HowToGet.Repository.Repositories
{
	public class UserRepository : IUserRepository
	{
		private const string ExtUsersCollectionName = "extUsers";
		private const string UsersCollectionName = "users";
		private const string UserLocationCollectionName = "userLocations";

		public string GetUserIdByExternalId(string externalId, ExternalAuthServices authService)
		{
			var collection = MongoHelper.Database.GetCollection<ExternalUserLink>(ExtUsersCollectionName);
			var query = Query.And(
							Query.EQ("extId", externalId),
							Query.EQ("as", Convert.ToInt32(authService))
							);
			var result = collection.FindOne(query);
			if (result == null)
				return null;
			return result.UserId.ToString();
		}

		public void DeleteUser(ObjectId userId)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			var query = Query.EQ("_id", userId); 
			collection.Remove(query);
		}

		public void AssociateExternalUser(ExternalUserLink userLink)
		{
			var collection = MongoHelper.Database.GetCollection<ExternalUserLink>(ExtUsersCollectionName);
			collection.Save(userLink);
		}

		public List<User> GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			var matches = collection.AsQueryable().Skip(pageIndex * pageSize).Take(pageSize).ToList();

			// execute second query to get total count
			totalRecords = (int)collection.Count();

			return matches.ToList();
		}

		public long GetNumberOfUsers()
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			return collection.Count();
		}

		public int GetNumberOfUsersOnline(DateTime compareTime)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			return collection.AsQueryable().Count(u => u.LastActivityDate > compareTime);
		}

		public User GetUser(string email, bool userIsOnline)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			var query = Query.EQ("lemail", email.ToLowerInvariant());
			if (userIsOnline)
			{
				// update the last-activity date
				var update = Update.Set("actdate", DateTime.UtcNow);
				var result = collection.FindAndModify(query, SortBy.Null, update, returnNew: true);
				if (!result.Ok)
					throw new InternalErrorException(result.ErrorMessage);

				if (result.ModifiedDocument == null)
					return null;

				return BsonSerializer.Deserialize<User>(result.ModifiedDocument);
			}
			
			return collection.FindOne(query);
		}

		public User GetUser(ObjectId userId, bool userIsOnline)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			if (userIsOnline)
			{
				// update the last-activity date
				var query = Query.EQ("_id", userId); 
				var update = Update.Set("actdate", DateTime.UtcNow);
				var result = collection.FindAndModify(query, SortBy.Null, update, returnNew: true);
				if (!result.Ok)
					throw new InternalErrorException(result.ErrorMessage);

				if (result.ModifiedDocument == null)
					return null;
				return BsonSerializer.Deserialize<User>(result.ModifiedDocument);
			}

			return collection.FindOneById(userId);
		}

		public string GetUserNameByEmail(string email)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			var query = Query.EQ("lemail", email.ToLowerInvariant());
			var user = collection.FindOne(query);
			return user == null ? null : user.Username;
		}

		public User GetUserByEmail(string email)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			var query = Query.EQ("lemail", email.ToLowerInvariant());
			return collection.FindOne(query);
		}

		public void UpdateUserPicture(ObjectId userId, string fileName)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			var query = Query.EQ("_id", userId);
			var update = Update.Set("pct", fileName);
			var result = collection.FindAndModify(query, SortBy.Null, update, true);
			if (!result.Ok)
				throw new InternalErrorException(result.ErrorMessage);
		}

		public void UpdateUserName(ObjectId userId, string userName)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			var query = Query.EQ("_id", userId);
			var update = Update.Set("uname", userName);
			var result = collection.FindAndModify(query, SortBy.Null, update, true);
			if (!result.Ok)
				throw new InternalErrorException(result.ErrorMessage);
		}

		public IEnumerable<User> GetUsersByName(string username)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			var query = Query.EQ("lname", username);
			return collection.Find(query);
		}

		public void UpdateUserRoles(ObjectId[] userIds, List<string> roles)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);

			var query = Query.In("_id", new BsonArray(userIds));
			var update = Update.AddToSetEachWrapped<string>("roles", roles);
			collection.Update(query, update, UpdateFlags.Multi);
		}

		public IEnumerable<User> GetUsersInRole(string role)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			var lowerRole = role.ToLowerInvariant();
			return collection.AsQueryable().Where(u => u.Roles.Contains(lowerRole));
		}

		public IEnumerable<User> FindUsersInRole(string role, ObjectId userId)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			var id = userId.ToString();
			return collection.AsQueryable().Where(u => u.Id == id && u.Roles.Contains(role));
		}

		public void RemoveUsersFromRoles(IEnumerable<ObjectId> usersIds, IEnumerable<string> roles)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);

			var query = Query.In("_id", new BsonArray(usersIds.ToArray()));
			var update = Update.PullAllWrapped("roles", roles);
			collection.Update(query, update, UpdateFlags.Multi);
		}

		UserLocation IUserRepository.GetLastLocation(string userId)
		{
			var collection = MongoHelper.Database.GetCollection(UserLocationCollectionName);
			var query = Query.EQ("uid", userId);
			var locations = collection.FindAs<UserLocation>(query).SetSortOrder(SortBy.Descending("t")).SetLimit(1);
			return locations.FirstOrDefault();
		}

		public void SaveLastLocation(UserLocation location)
		{
			var collection = MongoHelper.Database.GetCollection(UserLocationCollectionName);
			collection.Save(location);
		}

		public void UpdateUserData(User user)
		{
			var collection = MongoHelper.Database.GetCollection<User>(UsersCollectionName);
			var result = collection.Save(user);
			
			if(result == null)
				throw new InternalErrorException("Save to database did not return a status result");
			
			if(!result.Ok)
				throw new InternalErrorException(result.LastErrorMessage);
		}

	}
}