using System.Collections.Generic;

namespace HowToGet.Notifications.Interfaces
{
	public interface IConnectionsManager
	{
		int Count { get; }
		
		void Add(string userId, string connectionId);
		
		IEnumerable<string> Get(string userId);
		
		void Remove(string connectionId);
	}
}