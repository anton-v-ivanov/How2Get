using HowToGet.Models.Foursq;

namespace HowToGet.BusinessLogic.Interfaces
{
	public interface IFoursqProvider
	{
		bool IsCheckinExists(string id);
		void ProcessPush(FoursqPush push);
	}
}