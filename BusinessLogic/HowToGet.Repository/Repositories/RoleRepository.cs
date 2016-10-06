using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Users;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Driver.Builders;

namespace HowToGet.Repository.Repositories
{
	public class RoleRepository: IRoleRepository
	{
		private const string CollectionName = "roles";

		public IEnumerable<UserRole> GetAllRoles()
		{
			var collection = MongoHelper.Database.GetCollection<UserRole>(CollectionName);
			return collection.FindAll();
		}

		public bool RoleExists(string roleName)
		{
			var collection = MongoHelper.Database.GetCollection<UserRole>(CollectionName);
			var query = Query.EQ("role", roleName.ToLowerInvariant());
			var result =  collection.Find(query);
			return result != null && result.Any();
		}

		public void CreateRole(UserRole role)
		{
			var collection = MongoHelper.Database.GetCollection<UserRole>(CollectionName);
			collection.Insert(role);
		}

		public void DeleteRole(string role)
		{
			var collection = MongoHelper.Database.GetCollection<UserRole>(CollectionName);
			var query = Query.EQ("role", role.ToLowerInvariant());
			collection.Remove(query);
		}
	}
}