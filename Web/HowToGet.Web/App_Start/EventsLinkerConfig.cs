using System.Threading.Tasks;
using HowToGet.Analytics;
using HowToGet.Bonuses;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Analytics;

namespace HowToGet.Web.App_Start
{
	public class EventsLinkerConfig
	{
		private readonly IActionEvents _actionEvents;
		private readonly IAnalyticsActionTracker _analyticsTracker;
		private readonly IBonusActionTracker _bonusActionTracker;
		private readonly IBonusProvider _bonusProvider;

		public EventsLinkerConfig(IActionEvents actionEvents, IAnalyticsActionTracker analyticsTracker, IBonusActionTracker bonusActionTracker, IBonusProvider bonusProvider)
		{
			_actionEvents = actionEvents;
			_analyticsTracker = analyticsTracker;
			_bonusActionTracker = bonusActionTracker;
			_bonusProvider = bonusProvider;
		}

		public void RegisterEvents()
		{
			_actionEvents.UserAction += OnUserAction;
			_actionEvents.BonusReceived += _bonusProvider.OnBonusReceived;
		}

		private void OnUserAction(ActionBase action)
		{
			var bonusTask = new Task(() => _bonusActionTracker.Track(action));
			bonusTask.Start();

			var analyticTask = new Task(() => _analyticsTracker.Track(action));
			analyticTask.Start();
		}
	}
}