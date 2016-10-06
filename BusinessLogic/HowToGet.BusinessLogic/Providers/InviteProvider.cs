using System;
using System.Collections.Generic;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Notifications;
using HowToGet.Models.Users;
using HowToGet.Notifications.Interfaces;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;

namespace HowToGet.BusinessLogic.Providers
{
	public class InviteProvider : IInviteProvider
	{
		private readonly IInviteRepository _repository;
		private readonly IEmailNotificationProvider _notificationProvider;
		private readonly IUserProvider _userProvider;
		private readonly int _invitesPerUser;

		public InviteProvider(IInviteRepository repository, IEmailNotificationProvider notificationProvider, IUserProvider userProvider, int invitesPerUser)
		{
			_repository = repository;
			_notificationProvider = notificationProvider;
			_userProvider = userProvider;
			_invitesPerUser = invitesPerUser;
		}

		public bool CheckInviteCode(string inviteCode, out string inviteId)
		{
			var invite = _repository.FindInvite(inviteCode);
			if (invite == null || invite.Status == InviteStatus.Used)
			{
				inviteId = string.Empty;
				return false;
			}
			inviteId = invite.Id;
			return true;
		}

		public void GenerateInvites(string userId)
		{
			GenerateInvites(userId, _invitesPerUser);
		}

		public void GenerateInvites(string userId, int count)
		{
			if(count <= 0)
				throw new ArgumentException("Count must have positive value");

			var user = _userProvider.GetUserById(userId);
			if (user == null)
				throw new SecurityException(string.Format("User with id {0} doesn't exists", userId));

			for (var i = 0; i < count; i++)
			{
				while (true)
				{
					var code = GenerateInviteCode();
					if (_repository.FindInvite(code) != null)
						continue;

					var invite = new Invite
					{
						InviteCode = code,
						Status = InviteStatus.Generated,
						GeneratedUserId = userId,
					};
					_repository.SaveNewInvite(invite);
					break;
				}
			}
		}

		private static string GenerateInviteCode()
		{
			var random = new Random();
			return random.Next(100000000, 999999999).ToString("###-###-###");
		}

		public void UseInvite(string inviteId, string userId)
		{
			var objectId = MongoHelper.GetObjectIdFromString(inviteId, string.Format("{0} is not valid invite id", inviteId));
			var invite = _repository.GetInviteById(objectId);
			if (invite == null)
				throw new ObjectNotFoundException(string.Format("Invite {0} is not exists", inviteId));
			
			if(invite.Status == InviteStatus.Used)
				throw new SecurityException(string.Format("Invite {0} has already been used", inviteId));

			invite.Status = InviteStatus.Used;
			invite.UsedByUserId = userId;
			
			_repository.UpdateInvite(invite);
		}

		public IEnumerable<Invite> GetInvitesForUser(string userId)
		{
			var objectId = MongoHelper.GetObjectIdFromString(userId, string.Format("{0} is not valid user id", userId));
			return _repository.GetInvitesForUser(objectId);
		}
		
		public void SendInvite(string inviteId, string fromUserId, string toEmail)
		{
			var objectId = MongoHelper.GetObjectIdFromString(inviteId, string.Format("{0} is not valid invite id", inviteId));

			var invite = _repository.GetInviteById(objectId);
			if(invite == null)
				throw new ObjectNotFoundException(string.Format("Invite {0} is not exists", inviteId));
			
			if (invite.Status != InviteStatus.Generated)
				throw new ValidationException(string.Format("Invite {0} has invalid status: {1}", inviteId, invite.Status));

			_notificationProvider.Send(EmailNotificationType.Invite, fromUserId, new Dictionary<string, object> { { "toEmail", toEmail }, { "inviteCode", invite.InviteCode } });
			
			invite.Status = InviteStatus.Sended;
			invite.SendedTo = toEmail;

			_repository.UpdateInvite(invite);
		}

		public void CancelSendInvite(string inviteId)
		{
			var objectId = MongoHelper.GetObjectIdFromString(inviteId, string.Format("{0} is not valid invite id", inviteId));
			var invite = _repository.GetInviteById(objectId);
			if (invite == null)
				throw new ObjectNotFoundException(string.Format("Invite {0} is not exists", inviteId));
			
			if(invite.Status != InviteStatus.Sended)
				throw new ValidationException(string.Format("Invite {0} has not been sended", inviteId));
			
			invite.Status = InviteStatus.Generated;
			invite.SendedTo = string.Empty;

			_repository.UpdateInvite(invite);
		}
	}
}