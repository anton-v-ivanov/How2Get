using System.Collections.Generic;
using System.Linq;
using HowToGet.Models.Users;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace HowToGet.Repository.Repositories
{
	public class FailedEmailsRepository : IFailedEmailsRepository
	{
		private const string CollectionName = "failedEmails";

		public void InsertFailedEmail(UserEmail email)
		{
			var collection = MongoHelper.Database.GetCollection<UserEmail>(CollectionName);
			collection.Insert(email);
		}

		public IEnumerable<UserEmail> GetAll()
		{
			var collection = MongoHelper.Database.GetCollection<UserEmail>(CollectionName);
			return collection.FindAll().AsEnumerable();
		}

		public bool IsEmailExists(UserEmail email)
		{
			var collection = MongoHelper.Database.GetCollection<UserEmail>(CollectionName);
			var query = Query.And(Query.EQ("t", email.To), Query.EQ("s", email.Subject));
			return collection.FindOne(query) != null;
		}

		public void DeleteEmail(UserEmail email)
		{
			var collection = MongoHelper.Database.GetCollection<UserEmail>(CollectionName);
			var query = Query.EQ("_id", ObjectId.Parse(email.Id));
			collection.Remove(query);
		}
	}
}