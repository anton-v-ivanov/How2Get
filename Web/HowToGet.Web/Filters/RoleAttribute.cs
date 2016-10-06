using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Security;
using HowToGet.Security.Providers;

namespace HowToGet.Web.Filters
{
	public class RoleAttribute : ActionFilterAttribute
	{
		private readonly string _role;

		public RoleAttribute(string role)
		{
			_role = role;
		}

		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			string userId;
			if(!TokenProvider.TryGetUserIdFromAuthHeader(actionContext.Request, out userId))
				base.OnActionExecuting(actionContext);

			if (Roles.IsUserInRole(userId, _role) == false)
			{
				actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
					                         {
												 Content = new StringContent("InsufficientPermissions")
					                         };
				return;
			}

			base.OnActionExecuting(actionContext);
		}
	}
}