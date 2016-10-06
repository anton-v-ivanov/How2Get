using HowToGet.Models.Analytics;

namespace HowToGet.Analytics
{
	public interface IAnalyticsActionTracker
	{
		void Track(ActionBase action);
	}
}