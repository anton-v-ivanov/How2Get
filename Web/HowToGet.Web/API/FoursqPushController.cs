using System.Web.Http;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Foursq;

namespace HowToGet.Web.API
{
    public class FoursqPushController : ApiController
    {
	    private readonly IFoursqProvider _provider;

	    public FoursqPushController(IFoursqProvider provider)
	    {
		    _provider = provider;
	    }

		public void Post(FoursqPush push)
		{
			if (!_provider.IsCheckinExists(push.Checkin.Id))
			{
				_provider.ProcessPush(push);
			}
		}
    }
}
