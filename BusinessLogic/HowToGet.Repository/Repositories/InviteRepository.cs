using System.Collections.Generic;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Users;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace HowToGet.Repository.Repositories
{
	public class InviteRepository : IInviteRepository
	{
		const string CollectionName = "invites";

		public Invite FindInvite(string inviteCode)
		{
			var collection = MongoHelper.Database.GetCollection<Invite>(CollectionName);
			return collection.FindOne(Query.EQ("i", inviteCode));
		}

		public void SaveNewInvite(Invite invite)
		{
			var collection = MongoHelper.Database.GetCollection<Invite>(CollectionName);
			collection.Save(invite);
		}

		public void UpdateInvite(Invite invite)
		{
			var collection = MongoHelper.Database.GetCollection<Invite>(CollectionName);
			if (collection.FindOneById(ObjectId.Parse(invite.Id)) == null)
				throw new ObjectNotFoundException(string.Format("Invite {0} is not exists", invite.Id));

			collection.Save(invite);
		}

		public IEnumerable<Invite> GetInvitesForUser(ObjectId userId)
		{
			var collection = MongoHelper.Database.GetCollection<Invite>(CollectionName);
			var query = Query.And(Query.EQ("g", userId), Query.NE("s", InviteStatus.Sended));
			return collection.Find(query);
		}

		public Invite GetInviteById(ObjectId inviteId)
		{
			var collection = MongoHelper.Database.GetCollection<Invite>(CollectionName);
			return collection.FindOneById(inviteId);
		}
	}
}