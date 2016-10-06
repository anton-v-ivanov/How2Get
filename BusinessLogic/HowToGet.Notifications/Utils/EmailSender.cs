using System;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using HowToGet.Models.Users;
using HowToGet.Notifications.Configuration;
using HowToGet.Notifications.Providers;
using NLog;

namespace HowToGet.Notifications.Utils
{
    internal class EmailSender
    {
		private static Logger _logger;
		private static Logger CurrentLogger
		{
			get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger()); }
		}

		internal EmailSendResult Send(UserEmail email)
		{
			using (var client = new SmtpClient(NotificationsConfig.Instance.Host, NotificationsConfig.Instance.Port))
			{
				client.Credentials = new System.Net.NetworkCredential(NotificationsConfig.Instance.User, NotificationsConfig.Instance.Password);
				client.EnableSsl = true;
				var mail = new MailMessage(email.From, email.To, email.Subject, email.Body)
					           {
						           IsBodyHtml = true,
						           HeadersEncoding = Encoding.UTF8,
						           BodyEncoding = Encoding.UTF8,
								   SubjectEncoding = Encoding.UTF8,
					           };
				try
				{
					client.Send(mail);
					return EmailSendResult.Ok;
				}
				catch (Exception exception)
				{
					FailedEmailsProcessor.InsertFailedEmail(email);
					CurrentLogger.ErrorException(string.Format("Failed to send email to {0} with subj = {1}. Email inserted to queque with Id = {2}", 
						email.To, email.Subject, email.Id), exception);
					return EmailSendResult.Error;
				}
				
			}
		}

		internal void SendWithSmtp(UserEmail email)
		{
			var t1 = new Task(() => Send(email));
			t1.Start();
		}
    }
}
