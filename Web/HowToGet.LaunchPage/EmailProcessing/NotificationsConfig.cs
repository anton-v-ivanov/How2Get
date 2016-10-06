using System;
using System.Configuration;
using System.IO;

namespace LaunchPage.EmailProcessing
{
	internal static class NotificationsConfig
	{
		private static string _fromAddr;
		public static string FromAddr
		{
			get
			{
				if (string.IsNullOrEmpty(_fromAddr))
					_fromAddr = ConfigurationManager.AppSettings["EmaiFrom"];
				return _fromAddr;
			}
		}

		private static string _notificationsPath;
		public static string NotificationsPath
		{
			get
			{
				if (string.IsNullOrEmpty(_notificationsPath))
					_notificationsPath = ConfigurationManager.AppSettings["EmailNotificationsPath"];
				return _notificationsPath;
			}
		}

		private static string _subscribeEmailBody;
		public static string SubscribeEmailBody
		{
			get
			{
				if (string.IsNullOrEmpty(_subscribeEmailBody))
					_subscribeEmailBody = File.ReadAllText(NotificationsPath + "\\subscribe.html");
				return _subscribeEmailBody;
			}
		}

		private static string _host;
		public static string Host
		{
			get
			{
				if (string.IsNullOrEmpty(_host))
					_host = ConfigurationManager.AppSettings["EmailHost"];
				return _host;
			}
		}

		private static int _port;
		public static int Port
		{
			get
			{
				if (_port == 0)
					_port = Convert.ToInt32(ConfigurationManager.AppSettings["EmailPort"]);
				return _port;
			}
		}

		private static string _name;
		public static string Name
		{
			get
			{
				if (string.IsNullOrEmpty(_name))
					_name = ConfigurationManager.AppSettings["EmailUser"];
				return _name;
			}
		}

		private static string _password;
		public static string Password
		{
			get
			{
				if (string.IsNullOrEmpty(_password))
					_password = ConfigurationManager.AppSettings["EmailPassword"];
				return _password;
			}
		}
	}
}