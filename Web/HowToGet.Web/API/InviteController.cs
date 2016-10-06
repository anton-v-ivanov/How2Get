using System.Collections.Generic;
using System.Web.Http;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Users;
using HowToGet.Web.Filters;

namespace HowToGet.Web.API
{
	[ExceptionHandler]
	public class InviteController : ApiController
    {
	    private readonly IInviteProvider _inviteProvider;

	    public InviteController(IInviteProvider inviteProvider)
		{
			_inviteProvider = inviteProvider;
		}

	    [HttpGet]
		public string Check(string inviteCode)
	    {
		    string inviteId;
			if(_inviteProvider.CheckInviteCode(inviteCode, out inviteId))
				return inviteId;
		    throw new ObjectNotFoundException("UnknownInvite");
	    }

	    [HttpGet]
		[Authorize]
		public IEnumerable<Invite> Get()
		{
			return _inviteProvider.GetInvitesForUser(User.Identity.Name);
		}

		[HttpPost]
		[Authorize]
		public void Send(string inviteId, string email)
		{
			_inviteProvider.SendInvite(inviteId, User.Identity.Name, email);
		}

		[HttpDelete]
		[Authorize]
		public void Cancel(string inviteId)
		{
			_inviteProvider.CancelSendInvite(inviteId);
		}
    }
}
