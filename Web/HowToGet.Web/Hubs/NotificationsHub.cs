using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Notifications;
using HowToGet.Notifications.Interfaces;
using HowToGet.Security.Providers;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace HowToGet.Web.Hubs
{
	[HubName("announce")]
	public class NotificationsHub : Hub
	{
		private readonly ILifetimeScope _hubLifetimeScope;
		private readonly INotificationProvider _notificationProvider;
		private readonly IActionEvents _actionEvents;

		private const string AuthQueryString = "t";

		public NotificationsHub(ILifetimeScope lifetimeScope)
		{
			_hubLifetimeScope = lifetimeScope.BeginLifetimeScope();

			_notificationProvider = _hubLifetimeScope.Resolve<INotificationProvider>();
			_actionEvents = _hubLifetimeScope.Resolve<IActionEvents>();
			_actionEvents.NotificationCreated += OnNotificationCreated;
		}

		private void OnNotificationCreated(Notification notification)
		{
			Clients.Group(notification.UserId).addNotification(notification);
		}

		public void MarkRead(string notificationId)
		{
			_notificationProvider.MarkAsRead(notificationId);
			Clients.Group(GetUserId()).removeNotification(notificationId);
		}

		private void GetNotifications(string userId)
		{
			var notifications = _notificationProvider.GetNotifications(userId).ToArray();
			if (!notifications.Any())
				return;

			Clients.Group(userId).addNotification(notifications);
		}

		public override Task OnConnected()
		{
			var userId = GetUserId();
		
			Groups.Add(Context.ConnectionId, userId);
			
			new Task(() => GetNotifications(userId)).Start();

			return base.OnConnected();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && _hubLifetimeScope != null)
				_hubLifetimeScope.Dispose();

			base.Dispose(disposing);
		}

		private string GetUserId()
		{
			string userId = string.Empty;
			string token = Context.QueryString[AuthQueryString];
			if (!string.IsNullOrEmpty(token))
				userId = TokenProvider.GetUserIdFromTokenValue(token);
			return userId;
		}
	}
}