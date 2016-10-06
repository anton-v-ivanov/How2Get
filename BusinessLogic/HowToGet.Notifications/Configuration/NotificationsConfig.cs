using System.Configuration;

namespace HowToGet.Notifications.Configuration
{
	public class NotificationsConfig : ConfigurationSection
	{
		public static NotificationsConfig Instance { get; private set; }

		static NotificationsConfig()
		{
			if(Instance == null)
				Instance = (NotificationsConfig)ConfigurationManager.GetSection("notifications/email");
		}

		private static string _fromAddr;
		[ConfigurationProperty("from", IsRequired = true)]
		public string FromAddr
		{
			get
			{
				if (string.IsNullOrEmpty(_fromAddr))
					_fromAddr = (string)this["from"];
				return _fromAddr;
			}
		}

		private static string _welcomeFromAddr;
		[ConfigurationProperty("welcome-from", IsRequired = true)]
		public string WelcomeFromAddr
		{
			get
			{
				if (string.IsNullOrEmpty(_welcomeFromAddr))
					_welcomeFromAddr = (string)this["welcome-from"];
				return _welcomeFromAddr;
			}
		}

		private static string _notificationsPath;
		[ConfigurationProperty("notifications-path", IsRequired = true)]
		public string NotificationsPath
		{
			get
			{
				if (string.IsNullOrEmpty(_notificationsPath))
					_notificationsPath = (string)this["notifications-path"];
				return _notificationsPath;
			}
		}

		private static string _host;
		[ConfigurationProperty("host", IsRequired = true)]
		public string Host
		{
			get
			{
				if (string.IsNullOrEmpty(_host))
					_host = (string)this["host"];
				return _host;
			}
		}

		private static int _port;
		[ConfigurationProperty("port", IsRequired = true)]
		public int Port
		{
			get
			{
				if (_port == 0)
					_port = (int)this["port"];
				return _port;
			}
		}

		private static string _user;
		[ConfigurationProperty("user", IsRequired = true)]
		public string User
		{
			get
			{
				if (string.IsNullOrEmpty(_user))
					_user = (string)this["user"];
				return _user;
			}
		}

		private static string _password;
		[ConfigurationProperty("password", IsRequired = true)]
		public string Password
		{
			get
			{
				if (string.IsNullOrEmpty(_password))
					_password = (string)this["password"];
				return _password;
			}
		}
	}
}