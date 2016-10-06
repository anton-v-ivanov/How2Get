using System.Web.Http;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Web.Filters;
using HowToGet.Web.Models;

namespace HowToGet.Web.API
{
	[ExceptionHandler]
	public class TokenController : ApiController
    {
	    private readonly IOneTimeTokenProvider _oneTimeTokenProvider;

	    public TokenController(IOneTimeTokenProvider oneTimeTokenProvider)
	    {
		    _oneTimeTokenProvider = oneTimeTokenProvider;
	    }

	    [HttpGet]
		public AuthModel ValidateToken(string token)
		{
			var newToken = _oneTimeTokenProvider.Exchange(token);
			return new AuthModel { AuthToken = newToken };
		}

		[HttpDelete]
		public void Delete(string token)
		{
			_oneTimeTokenProvider.MarkAsUsed(token);
		}
    }
}
