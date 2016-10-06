using System;
using HowToGet.Models.Users;

namespace HowToGet.Web.Models
{
	public class LaunchSubscriptionModel
	{
		public string Email { get; set; }

		public string Time { get; set; }

		public string Referrer { get; set; }

		public string Ip { get; set; }

		public LaunchSubscriptionModel(LaunchSubscription subscription)
		{
			Email = subscription.Email;
			Time = subscription.Time.ToString("dd.MM.yy HH:mm:ss");
			Ip = subscription.Ip;
		} 
	}
}