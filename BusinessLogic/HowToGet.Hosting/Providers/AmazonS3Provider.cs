using Amazon.S3;
using Amazon.S3.Model;

namespace HowToGet.Hosting.Providers
{
	public class AmazonS3Provider
	{
		private readonly string _bucketName;
		private readonly string _folderName;

		public AmazonS3Provider(string bucketName, string folderName)
		{
			_bucketName = bucketName;
			_folderName = folderName;
		}

		public void UploadFile(string fileName, string filePath)
		{
			var client = new AmazonS3Client();
			var request = new PutObjectRequest()
							.WithBucketName(_bucketName)
							.WithKey(_folderName + "/" + fileName)
							.WithCannedACL(S3CannedACL.PublicRead)
							.WithFilePath(filePath);
			client.PutObject(request);

		}
	}
}