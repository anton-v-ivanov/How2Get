namespace HowToGet.BusinessLogic.Interfaces
{
	public interface IOneTimeTokenProvider
	{
		string Generate(string userId);
		void MarkAsUsed(string token);
		string Exchange(string token);
	}
}