using System.Collections.Generic;
using HowToGet.Models.Bonuses;
using HowToGet.Models.Dictionaries;

namespace HowToGet.Bonuses
{
	public interface IBonusProvider
	{
		IEnumerable<Bonus> GetBonuses(string userId);
		
		void OnBonusReceived(string userid, BonusType bonustype);
	}
}