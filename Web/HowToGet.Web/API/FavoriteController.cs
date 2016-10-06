using System.Collections.Generic;
using System.Web.Http;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Routes;

namespace HowToGet.Web.API
{
	[Authorize]
	public class FavoriteController : ApiController
    {
		private readonly IRouteProvider _routeProvider;

		public FavoriteController(IRouteProvider routeProvider)
		{
			_routeProvider = routeProvider;
		}

		[HttpPost]
		public void MarkRouteAsFavorite(string routeId)
		{
			var userId = User.Identity.Name;
			_routeProvider.MarkRouteAsFavorite(routeId, userId);
		}

		[HttpGet]
		public List<Route> GetFavoriteRoutes()
		{
			var userId = User.Identity.Name;
			return _routeProvider.GetFavoriteRoutesForUser(userId);
		}

		[HttpDelete]
		public void RemoveFromFavorite(string routeId)
		{
			var userId = User.Identity.Name;
			_routeProvider.RemoveFromFavorite(routeId, userId);
		}
    }
}
