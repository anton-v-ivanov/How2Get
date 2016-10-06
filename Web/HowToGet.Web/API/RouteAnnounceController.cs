using System.Linq;
using System.Web.Http;
using System.Web.Security;
using HowToGet.BusinessLogic.Configuration;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.BusinessLogic.Validators;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Users;
using HowToGet.Security.Helpers;
using HowToGet.Web.Filters;
using HowToGet.Web.Models;
using HowToGet.Web.Helpers;

namespace HowToGet.Web.API
{
	[ExceptionHandler]
    public class RouteAnnounceController : ApiController
    {
		private readonly IRouteAnnounceProvider _routeAnnounceProvider;
		private readonly ICityValidator _cityValidator;

		public RouteAnnounceController(IRouteAnnounceProvider routeAnnounceProvider, ICityValidator cityValidator)
		{
			_routeAnnounceProvider = routeAnnounceProvider;
			_cityValidator = cityValidator;
		}

		[HttpGet]
		[Authorize]
		[ActionName("self")]
		public bool IsUserSubscribed(string origin, string destination)
		{
			var userId = User.Identity.Name;
			return _routeAnnounceProvider.IsUserSubscribed(origin, destination, userId);
		}

		[HttpGet]
		[ActionName("list")]
		public RouteSubscribersModel GetSubscribed(string origin, string destination)
		{
			_cityValidator.ValidateCity(CityValidator.ValidateCityType.Origin, origin);
			_cityValidator.ValidateCity(CityValidator.ValidateCityType.Destination, destination);
			
			int count;
			var usersIds = _routeAnnounceProvider.GetSubscribedUserIds(origin, destination, out count);
			var result = usersIds.Select(userId => Membership.GetUser((object) userId, true) as MembershipUserEx)
							.Select(user => new ShortUserDataModel(user, ImagesConfig.Instance.VirtualImageFolderName));
			
			return new RouteSubscribersModel{ Users = result, Count = count };
		}

		[HttpPost]
		public void SubscribeToRoute(RouteAnnounceParamsModel paramsModel)
		{
			if(!EmailValidator.Validate(paramsModel.Email))
				throw new ValidationException(string.Format("{0} is not valid email", paramsModel.Email));

			var userId = string.Empty;
			if (User != null && !string.IsNullOrEmpty(User.Identity.Name))
			{
				userId = User.Identity.Name;
			}
			if (string.IsNullOrEmpty(userId))
			{
				var users = Membership.FindUsersByEmail(paramsModel.Email);
				if(users != null && users.Count > 0)
					userId = users.First().ProviderUserKey.ToString();
			}
			_routeAnnounceProvider.AddSubscription(paramsModel.Origin, paramsModel.Destination, paramsModel.Email, userId);
		}

		[HttpDelete]
		public void UnsubscribeFromRoute(RouteAnnounceParamsModel paramsModel)
		{
			_cityValidator.ValidateCity(CityValidator.ValidateCityType.Origin, paramsModel.Origin);
			_cityValidator.ValidateCity(CityValidator.ValidateCityType.Destination, paramsModel.Destination);
			
			_routeAnnounceProvider.RemoveSubscription(paramsModel.Origin, paramsModel.Destination, paramsModel.Email);
		}
    }
}
