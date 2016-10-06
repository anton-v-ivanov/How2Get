using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;

namespace HowToGet.Web.Helpers
{
	public class ClientHelper
	{
		internal static string GetClientIp(HttpRequestMessage request)
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
				var prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
				return prop.Address;
			}
			return null;
		}
		
		internal static string GetReferrer(HttpRequestMessage request)
		{
			if (request == null)
				return null;

			if (request.Properties.ContainsKey("MS_HttpContext"))
			{
				var cont = request.Properties["MS_HttpContext"] as HttpContextWrapper;
				if (cont != null && cont.Request.UrlReferrer != null)
					return cont.Request.UrlReferrer.ToString();
			}
			if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
			{
				var prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
				return prop.Address;
			}
			return null;
		}
 
	}
}