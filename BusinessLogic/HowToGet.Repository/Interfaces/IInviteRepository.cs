using System.Collections.Generic;
using HowToGet.Models.Users;
using MongoDB.Bson;

namespace HowToGet.Repository.Interfaces
{
	public interface IInviteRepository
	{
		Invite FindInvite(string inviteCode);

		void SaveNewInvite(Invite invite);
		
		IEnumerable<Invite> GetInvitesForUser(ObjectId userId);

		void UpdateInvite(Invite invite);
	
		Invite GetInviteById(ObjectId inviteId);
	}
}