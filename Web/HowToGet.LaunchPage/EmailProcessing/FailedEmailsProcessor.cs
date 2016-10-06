using System.Timers;
using NLog;

namespace LaunchPage.EmailProcessing
{
	public class FailedEmailsProcessor
	{
		private static Logger _logger;
		private static Logger CurrentLogger
		{
			get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger()); }
		}

		private static FailedEmailsRepository _failedEmailsRepository;
		private static FailedEmailsRepository FailedEmailsRepository
		{
			get { return _failedEmailsRepository ?? (_failedEmailsRepository = new FailedEmailsRepository()); }
		}

		private static Timer _timer;

		private static readonly object SyncObj = new object();

		private static void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			lock (SyncObj)
			{
				var emails = FailedEmailsRepository.GetAll();
				var emailSender = new EmailSender();
				foreach (var email in emails)
				{
					CurrentLogger.Info("Trying to send notification with subj '{0}' to {1}", email.Subject, email.To);
					if (emailSender.Send(email) == EmailSendResult.Ok)
					{
						FailedEmailsRepository.DeleteEmail(email);
						CurrentLogger.Info("Notification with subj '{0}' to {1} has successfully sent and was deleted from queue", email.Subject, email.To);
					}
				}
			}
		}

		public static void InsertFailedEmail(UserEmail email)
		{
			if (!FailedEmailsRepository.IsEmailExists(email))
			{
				FailedEmailsRepository.InsertFailedEmail(email);
				CurrentLogger.Info("Notification with subj '{0}' to {1} has inserted to failed mails queue", email.Subject, email.To);
			}
		}

		public static void StartEmailQueue()
		{
			if (_timer != null)
				return;

			CurrentLogger.Info("Failed email queue started");

			// 10 min.
			//Timer = new Timer(1000 * 60 * 10);
			_timer = new Timer(1000 * 10);
			_timer.Elapsed += TimerOnElapsed;
			_timer.Start();

		}
	}
}