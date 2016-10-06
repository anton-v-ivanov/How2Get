using HowToGet.Models.Security;

namespace HowToGet.Repository.Interfaces
{
	public interface IOneTimeTokenRepository
	{
		OneTimeToken Check(string token);

		void Save(OneTimeToken token);

		void MarkAsUsed(string token);
	}
}