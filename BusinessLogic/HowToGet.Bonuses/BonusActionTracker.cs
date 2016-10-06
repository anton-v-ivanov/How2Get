using System.Collections.Generic;
using HowToGet.Bonuses.BonusDefinitions;
using HowToGet.BusinessLogic.Interfaces;
using HowToGet.Models.Analytics;
using HowToGet.Models.Dictionaries;

namespace HowToGet.Bonuses
{
	public class BonusActionTracker : IBonusActionTracker
    {
		private readonly IEnumerable<IBonus> _bonuses;
		private readonly IActionEvents _actionEvents;

		public BonusActionTracker(IEnumerable<IBonus> bonuses, IActionEvents actionEvents)
		{
			_bonuses = bonuses;
			_actionEvents = actionEvents;
		}

		public void Track(ActionBase action)
		{
			foreach (var bonus in _bonuses)
			{
				if (bonus.SourceActionType == action.GetType() && bonus.IsValid(action))
				{
					_actionEvents.OnBonusReceived(action.UserId, BonusType.Registration);
				}
			}
		}
    }
}
