using System.Web.Http;

namespace HowToGet.Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute
				(
					"api",
					"api/{controller}/{action}/{id}", 
					new { id = RouteParameter.Optional, action = RouteParameter.Optional }
				);
        }
    }
}
