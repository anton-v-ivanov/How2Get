using HowToGet.Models.Analytics;

namespace HowToGet.Bonuses
{
	public interface IBonusActionTracker
	{
		void Track(ActionBase action); 
	}
}