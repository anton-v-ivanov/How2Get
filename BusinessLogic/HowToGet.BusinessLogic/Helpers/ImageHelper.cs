using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace HowToGet.BusinessLogic.Helpers
{
	public class ImageHelper
	{
		public static string Resize(string filePath, int width, int height, bool preserveAspectRatio = true)
		{
			using (var image = Image.FromFile(filePath))
			{
				if (image.Width > width || image.Height > height)
				{
					int newWidth;
					int newHeight;
					if (preserveAspectRatio)
					{
						int originalWidth = image.Width;
						int originalHeight = image.Height;
						float percentWidth = width/(float) originalWidth;
						float percentHeight = height/(float) originalHeight;
						float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
						newWidth = (int) (originalWidth*percent);
						newHeight = (int) (originalHeight*percent);
					}
					else
					{
						newWidth = width;
						newHeight = height;
					}
					Image newImage = new Bitmap(newWidth, newHeight);
					using (var graphicsHandle = Graphics.FromImage(newImage))
					{
						graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
						graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
					}
					var fileName = Path.GetFileNameWithoutExtension(filePath);
					var ext = Path.GetExtension(filePath);
					var path = Path.GetDirectoryName(filePath);
					string newFile = string.Format("{0}\\{1}{2}", path, fileName + "_rs", ext);
					newImage.Save(newFile, ImageFormat.Jpeg);
					return newFile;
				}
				return filePath;
			}
		}
	}
}