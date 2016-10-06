using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using HowToGet.Security.Providers;

namespace HowToGet.Security.HttpModule
{
	public class TokenAuthenticationModule : IHttpModule
	{
		public void Init(HttpApplication context)
		{
			context.AuthenticateRequest += AuthenticateRequests;
		}

		public void Dispose()
		{
		}

		private static void AuthenticateRequests(object sender, EventArgs e)
		{
			string userId;
			if (!TokenProvider.TryGetUserIdFromAuthHeader(HttpContext.Current.Request, out userId)) 
				return;

			Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(userId), Roles.GetRolesForUser(userId));
			HttpContext.Current.User = Thread.CurrentPrincipal;
		}

	}
}