using System.Configuration;

namespace HowToGet.BusinessLogic.Configuration
{
	public class ImagesConfig : ConfigurationSection
	{
		public static ImagesConfig Instance { get; private set; }
		static ImagesConfig()
		{
			Instance = (ImagesConfig) ConfigurationManager.GetSection("images");
		}

		private static string _imagePath;
		[ConfigurationProperty("path", IsRequired = true)]
		public string ImagePath
		{
			get
			{
				if (string.IsNullOrEmpty(_imagePath))
					_imagePath = (string)this["path"];
				return _imagePath;
			}
		}

		private static string _virtualImageFolderName;
		[ConfigurationProperty("virtual-folder-name", IsRequired = true)]
		public string VirtualImageFolderName
		{
			get
			{
				if (string.IsNullOrEmpty(_virtualImageFolderName))
					_virtualImageFolderName = (string)this["virtual-folder-name"];
				return _virtualImageFolderName;
			}
		}

		private static int _maxWidth;
		[ConfigurationProperty("max-thumb-width", IsRequired = true)]
		public int MaxWidth
		{
			get
			{
				if (_maxWidth == 0)
					_maxWidth = (int)this["max-thumb-width"];
				return _maxWidth;
			}
		}

		private static int _maxHeight;
		[ConfigurationProperty("max-thumb-height", IsRequired = true)]
		public int MaxHeight
		{
			get
			{
				if (_maxHeight == 0)
					_maxHeight = (int)this["max-thumb-height"];
				return _maxHeight;
			}
		}

		private static string _bucketName;
		[ConfigurationProperty("bucket-name", IsRequired = true)]
		public string BucketName
		{
			get
			{
				if (string.IsNullOrEmpty(_bucketName))
					_bucketName = (string)this["bucket-name"];
				return _bucketName;
			}
		}

		private static string _buckerFolderName;
		[ConfigurationProperty("bucket-folder-name", IsRequired = true)]
		public string BuckerFolderName
		{
			get
			{
				if (string.IsNullOrEmpty(_buckerFolderName))
					_buckerFolderName = (string)this["bucket-folder-name"];
				return _buckerFolderName;
			}
		}
	}
}