using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace LaunchPage.EmailProcessing
{
	public class FailedEmailsRepository
	{
		public void InsertFailedEmail(UserEmail email)
		{
			var collection = MongoHelper.Database.GetCollection<UserEmail>("failedSubscribeEmails");
			collection.Insert(email);
		}

		public IEnumerable<UserEmail> GetAll()
		{
			var collection = MongoHelper.Database.GetCollection<UserEmail>("failedSubscribeEmails");
			return collection.FindAll().AsEnumerable();
		}

		public bool IsEmailExists(UserEmail email)
		{
			var collection = MongoHelper.Database.GetCollection<UserEmail>("failedSubscribeEmails");
			var query = Query.And(Query.EQ("t", email.To), Query.EQ("s", email.Subject));
			return collection.FindOne(query) != null;
		}

		public void DeleteEmail(UserEmail email)
		{
			var collection = MongoHelper.Database.GetCollection<UserEmail>("failedSubscribeEmails");
			var query = Query.EQ("_id", ObjectId.Parse(email.Id));
			collection.Remove(query);
		}
 
	}
}