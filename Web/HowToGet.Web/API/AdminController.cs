using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Users;
using HowToGet.Web.Filters;
using HowToGet.Web.Models;
using NLog;

namespace HowToGet.Web.API
{
	[ExceptionHandler]
	[Authorize]
	public class AdminController : ApiController
    {
		private readonly ISubscriptionsProvider _subscriptionsProvider;
		private readonly IInviteProvider _inviteProvider;

		private static Logger _logger;
		private static Logger CurrentLogger
		{
			get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger()); }
		}

		public AdminController(ISubscriptionsProvider subscriptionsProvider, IInviteProvider inviteProvider)
		{
			_subscriptionsProvider = subscriptionsProvider;
			_inviteProvider = inviteProvider;
		}

		// GET api/admin/subscriptions
		[ActionName("subscriptions")]
		[HttpGet]
		//[Role("Admin")]
		public IEnumerable<LaunchSubscriptionModel> GetSubscriptions()
		{
			int omit = 0, count = 10000;
			return _subscriptionsProvider.GetAllSubscriptions(omit, count).Select(s => new LaunchSubscriptionModel(s));
		}

		[HttpPost]
		[ActionName("invite-gen")]
		public IEnumerable<Invite> GenerateInvites(GenerateInviteModel model)
		{
			_inviteProvider.GenerateInvites(model.UserId, model.Count);
			return _inviteProvider.GetInvitesForUser(model.UserId);
		}

		[HttpGet]
		[ActionName("invite-show")]
		public IEnumerable<Invite> ShowInvites(string userId)
		{
			return _inviteProvider.GetInvitesForUser(userId);
		}
    }
}
