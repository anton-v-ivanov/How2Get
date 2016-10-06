using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Users;
using HowToGet.Security.Providers;
using HowToGet.Web.Filters;
using HowToGet.Web.Models;

namespace HowToGet.Web.API
{
    [ExceptionHandler]
    public class AuthController : ApiController
    {
        //[Https]
		[HttpPost]
        public AuthModel Login(LoginModel loginModel)
        {
            if (Membership.ValidateUser(loginModel.Email, loginModel.Password))
            {
	            var user = Membership.GetUser(loginModel.Email, true) as MembershipUserEx;
				return new AuthModel(user);
            }
	        throw new SecurityException("IncorrectLoginOrPassword");
        }

		[Authorize]
		[HttpDelete]
		public HttpResponseMessage Logout()
        {
			var userId = User.Identity.Name;
	        var token = ControllerContext.Request.Headers.GetValues("Authorization").First();
	        TokenProvider.RemoveToken(userId, token);
			return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
