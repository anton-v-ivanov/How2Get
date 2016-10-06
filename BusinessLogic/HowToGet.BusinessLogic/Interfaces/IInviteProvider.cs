using System.Collections.Generic;
using HowToGet.Models.Users;

namespace HowToGet.BusinessLogic.Interfaces
{
	public interface IInviteProvider
	{
		bool CheckInviteCode(string inviteCode, out string inviteId);

		void GenerateInvites(string userId);

		void GenerateInvites(string userId, int count);
		
		void UseInvite(string inviteId, string userId);

		IEnumerable<Invite> GetInvitesForUser(string userId);

		void SendInvite(string inviteId, string fromUserId, string toEmail);
		
		void CancelSendInvite(string inviteId);
	}
}