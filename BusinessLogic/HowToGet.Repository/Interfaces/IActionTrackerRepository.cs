using HowToGet.Models.Analytics;

namespace HowToGet.Repository.Interfaces
{
	public interface IActionTrackerRepository
	{
		void RecordAction(ActionBase action);
	}
}