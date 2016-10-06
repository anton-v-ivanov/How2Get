using System;
using LaunchPage.EmailProcessing;
using MongoDB.Driver.Builders;

namespace LaunchPage.Providers
{
	public enum CreateResult
	{
		Ok,
		EmailAlreadyExists
	}

	public class EmailProvider
	{
		public CreateResult CreateSubscription(string email, string referrer, string ip)
		{
			var collection = MongoHelper.Database.GetCollection<Subscription>("subscriptions");
			var query = Query.EQ("lemail", email.ToLowerInvariant());
			var subscription = collection.FindOne(query);
			if (subscription != null)
				return CreateResult.EmailAlreadyExists;
			
			subscription = new Subscription
				               {
					               Email = email,
								   LowerCaseEmail = email.ToLowerInvariant(),
								   Time = DateTime.UtcNow,
								   Referrer = referrer,
								   Ip = ip
				               };
			
			collection.Insert(subscription);
			return CreateResult.Ok;
		}

		public void SendEmail(string email)
		{
			var body = NotificationsConfig.SubscribeEmailBody;

			new EmailSender().SendWithSmtp(new UserEmail
			{
				From = NotificationsConfig.FromAddr,
				To = email,
				Subject = "Thank you for signing up! From how2get.to",
				Body = body
			});
		}
	}
}