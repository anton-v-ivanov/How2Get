using System.IO;
using System.Threading.Tasks;
using HowToGet.BusinessLogic.Configuration;
using HowToGet.Hosting.Helpers;
using HowToGet.Hosting.Interfaces;

namespace HowToGet.Hosting.Providers
{
	public class FileUploadProvider : IUploadToHostingProvider
	{
		public string PrepareAndUploadImage(string file)
		{
			var filePath = ImageHelper.Resize(file, ImagesConfig.Instance.MaxWidth, ImagesConfig.Instance.MaxHeight);
			var fileName = Path.GetFileName(filePath);

			new AmazonS3Provider(ImagesConfig.Instance.BucketName, ImagesConfig.Instance.BuckerFolderName).UploadFile(fileName, filePath);

			var t1 = new Task(() => File.Delete(file));
			var t2 = new Task(() => File.Delete(filePath));
			t1.Start();
			t2.Start();
			
			return fileName;
		}
	}
}