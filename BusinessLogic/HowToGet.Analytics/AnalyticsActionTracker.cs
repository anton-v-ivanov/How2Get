using System.Threading.Tasks;
using HowToGet.Models.Analytics;
using HowToGet.Repository.Interfaces;

namespace HowToGet.Analytics
{
	public class AnalyticsActionTracker : IAnalyticsActionTracker
	{
		private readonly IActionTrackerRepository _repository;

		public AnalyticsActionTracker(IActionTrackerRepository repository)
		{
			_repository = repository;
		}

		public void Track(ActionBase action)
		{
			var t = new Task(() => _repository.RecordAction(action));
			t.Start();
		}
	}
}