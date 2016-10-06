using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Web.Security;
using NLog;

namespace HowToGet.Web.Filters
{
	public class ExceptionHandler: ExceptionFilterAttribute 
	{
		private static Logger _logger;
		private static Logger CurrentLogger
		{
			get { return _logger ?? (_logger = LogManager.GetCurrentClassLogger()); }
		}

        public override void OnException(HttpActionExecutedContext context)
        {
	        switch (context.Exception.GetType().Name)
	        {
				case "SecurityException":
					context.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
					{
						Content = new StringContent(context.Exception.Message)
					};
			        break;
				case "ObjectNotFoundException":
					context.Response = new HttpResponseMessage(HttpStatusCode.NotFound)
					{
						Content = new StringContent(context.Exception.Message)
					};
			        break;
				case "InvalidObjectIdException":
					context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
					{
						Content = new StringContent(context.Exception.Message)
					};
			        break;
				case "ValidationException":
					context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
					{
						Content = new StringContent(context.Exception.Message)
					};
			        break;
				case "MembershipCreateUserException":
					context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
					{
						Content = new StringContent(((MembershipCreateUserException)context.Exception).StatusCode.ToString())
					};
			        break;
				case "UnsupportedMediaTypeException":
					context.Response = new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType)
					{
						Content = new StringContent(context.Exception.Message)
					};
					break;
				default:
					context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
			        break;
	        }
			CurrentLogger.ErrorException("API Error", context.Exception);
        }
	}
}