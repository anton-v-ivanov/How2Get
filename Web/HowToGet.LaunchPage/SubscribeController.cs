using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using LaunchPage.Models;
using LaunchPage.Providers;
using NLog;

namespace LaunchPage
{
	public class SubscribeController : ApiController
	{
		private static Logger _logger;
		private static Logger CurrentLogger
		{
			get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger()); }
		}

		private static EmailProvider _emailProvider;
		public EmailProvider EmailProvider
		{
			get { return _emailProvider ?? (_emailProvider = new EmailProvider()); }
		}

		[ActionName("create")]
		[HttpPost]
		public HttpResponseMessage Subscribe(SubscribeModel model)
		{
			CurrentLogger.Info("Subcription create request. Email: {0}", model.Email);

			string ip = GetClientIp(Request);
			var result = EmailProvider.CreateSubscription(model.Email, model.Referrer, ip);
			if (result == CreateResult.EmailAlreadyExists)
			{
				CurrentLogger.Info("Email {0} already exists", model.Email);
				return new HttpResponseMessage(HttpStatusCode.Conflict);
			}

			EmailProvider.SendEmail(model.Email);
			CurrentLogger.Info("Subscription created. Email to {0} sent", model.Email);
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
		
		private string GetClientIp(HttpRequestMessage request)
		{
			if (request == null)
				return null;

			if (request.Properties.ContainsKey("MS_HttpContext"))
			{
				var cont = request.Properties["MS_HttpContext"] as HttpContextWrapper;
				if (cont != null)
					return cont.Request.UserHostAddress;
			}
			if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
			{
				var prop = (RemoteEndpointMessageProperty)Request.Properties[RemoteEndpointMessageProperty.Name];
				return prop.Address;
			}
			return null;
		}
	}
}