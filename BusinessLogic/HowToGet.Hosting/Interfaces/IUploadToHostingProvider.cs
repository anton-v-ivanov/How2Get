namespace HowToGet.Hosting.Interfaces
{
	public interface IUploadToHostingProvider
	{
		string PrepareAndUploadImage(string fileName);
	}
}