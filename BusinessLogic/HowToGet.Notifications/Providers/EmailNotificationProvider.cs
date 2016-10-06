using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HowToGet.Models.Notifications;
using HowToGet.Models.Users;
using HowToGet.Notifications.Configuration;
using HowToGet.Notifications.Interfaces;
using HowToGet.Notifications.Utils;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;

namespace HowToGet.Notifications.Providers
{
	public class EmailNotificationProvider : IEmailNotificationProvider
	{
		private readonly IUserRepository _userRepository;
		private readonly string _notificationPath;
		private readonly ConcurrentDictionary<EmailNotificationType, string> _emailBodies;

		public EmailNotificationProvider(IUserRepository userRepository, string notificationPath)
		{
			_userRepository = userRepository;
			_notificationPath = notificationPath;
			_emailBodies = new ConcurrentDictionary<EmailNotificationType, string>();
		}

		public void Send(EmailNotificationType emailNotificationType, string initiatorId, Dictionary<string, object> parameters)
		{
			var objectId = MongoHelper.GetObjectIdFromString(initiatorId, string.Format("{0} is not valid user id", initiatorId));
			var user = _userRepository.GetUser(objectId, false);
			Send(emailNotificationType, user, parameters);
		}

		public void Send(EmailNotificationType emailNotificationType, User initiator, Dictionary<string, object> parameters)
		{
			var from = NotificationsConfig.Instance.FromAddr;
			var toEmail = initiator.Email;
			
			if(parameters == null)
				parameters = new Dictionary<string, object>();

			parameters.Add("userName", initiator.Username);
			parameters.Add("email", initiator.Email);
			parameters.Add("userId", initiator.Id);
			var body = LoadBody(emailNotificationType);
			body = FillBody(body, parameters);

			string subject = GetSubject(emailNotificationType);

			switch (emailNotificationType)
			{
				case EmailNotificationType.Registered:
				case EmailNotificationType.RegisteredExternal:
				case EmailNotificationType.RegisteredWithAutoPassword:
					from = NotificationsConfig.Instance.WelcomeFromAddr;
					break;
			}

			new EmailSender().SendWithSmtp(new UserEmail
			{
				From = from,
				To = toEmail,
				Subject = subject,
				Body = body
			});
		}

		public void SendUnregistered(EmailNotificationType emailNotificationType, string email, Dictionary<string, object> parameters)
		{
			var from = NotificationsConfig.Instance.FromAddr;
			var toEmail = email;

			if (parameters == null)
				parameters = new Dictionary<string, object>();

			var body = LoadBody(emailNotificationType);
			body = FillBody(body, parameters);

			string subject = GetSubject(emailNotificationType);

			switch (emailNotificationType)
			{
				case EmailNotificationType.Registered:
				case EmailNotificationType.RegisteredExternal:
				case EmailNotificationType.RegisteredWithAutoPassword:
					from = NotificationsConfig.Instance.WelcomeFromAddr;
					break;
			}

			new EmailSender().SendWithSmtp(new UserEmail
			{
				From = from,
				To = toEmail,
				Subject = subject,
				Body = body
			});
		}

		private static string GetSubject(EmailNotificationType emailNotificationType)
		{
			switch (emailNotificationType)
			{
				case EmailNotificationType.Registered:
				case EmailNotificationType.RegisteredExternal:
				case EmailNotificationType.RegisteredWithAutoPassword:
					return "Welcome to rutta.me!";

				case EmailNotificationType.ForgotPassword:
					return "Password reset for rutta.me";

				case EmailNotificationType.Invite:
					return "Invitation code to rutta.me";

				case EmailNotificationType.TripHappened:
					return "Save your route on rutta.me!";

				case EmailNotificationType.RouteAnnounce:
					return "Your subscription to route on rutta.me";
				
				case EmailNotificationType.RouteAddedAnnounce:
					return "Route added on rutta.me!";
				
				default:
					throw new ArgumentOutOfRangeException("emailNotificationType");
			}
		}

		private string LoadBody(EmailNotificationType emailNotificationType)
		{
			string result;
			if (_emailBodies.TryGetValue(emailNotificationType, out result))
				return result;

			result = File.ReadAllText(_notificationPath + string.Format("\\{0}.html", emailNotificationType));
			_emailBodies.AddOrUpdate(emailNotificationType, result, (type, s) => result);
			return result;
		}
		
		private static string FillBody(string body, Dictionary<string, object> parameters)
		{
			return parameters.Aggregate(body, (current, parameter) => current.Replace(string.Format("%{0}%", parameter.Key.ToLowerInvariant()), parameter.Value.ToString()));
		}
	}
}