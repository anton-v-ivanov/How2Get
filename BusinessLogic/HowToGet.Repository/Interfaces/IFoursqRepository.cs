using HowToGet.Models.Foursq;

namespace HowToGet.Repository.Interfaces
{
	public interface IFoursqRepository
	{
		bool IsCheckinExists(string id);
		
		CheckinInfo GetUserLastCheckin(string userId);

		void SaveCheckin(CheckinInfo checkin);
	}
}