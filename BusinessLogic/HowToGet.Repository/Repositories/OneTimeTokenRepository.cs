using HowToGet.Models.Security;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Driver.Builders;

namespace HowToGet.Repository.Repositories
{
	public class OneTimeTokenRepository : IOneTimeTokenRepository
	{
		private const string CollectionName = "oneTimeTokens";

		public OneTimeToken Check(string token)
		{
			var collection = MongoHelper.Database.GetCollection<OneTimeToken>(CollectionName);
			var query = Query.EQ("t", token.ToLowerInvariant());
			var tokenObj = collection.FindOne(query);
			if (tokenObj == null || tokenObj.State != OneTimeTokenState.Active)
				return null;
			return tokenObj;
		}

		public void Save(OneTimeToken token)
		{
			var collection = MongoHelper.Database.GetCollection<OneTimeToken>(CollectionName);
			collection.Save(token);
		}

		public void MarkAsUsed(string token)
		{
			var collection = MongoHelper.Database.GetCollection<OneTimeToken>(CollectionName);
			var tokenObj = Check(token);
			if (tokenObj == null) 
				return;
			tokenObj.State = OneTimeTokenState.Used;
			collection.Save(tokenObj);
		}

	}
}