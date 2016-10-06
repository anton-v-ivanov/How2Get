using System.Net.Http;
using System.Text.RegularExpressions;
using HowToGet.Models.Exceptions;

namespace HowToGet.Web.Helpers
{
	public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider 
	{
		private static readonly Regex ExtRegex = new Regex(@"\.\w+", RegexOptions.Compiled);

		private readonly string _userId;

		public CustomMultipartFormDataStreamProvider(string rootPath, string userId)
			:base(rootPath)
		{
			_userId = userId;
		}

		public override string GetLocalFileName(System.Net.Http.Headers.HttpContentHeaders headers)
		{
			if (!headers.ContentType.MediaType.Contains("image") || headers.ContentType.MediaType == "image/gif")
				throw new UnsupportedMediaTypeException(string.Format("UnsupportedMediaType: " + headers.ContentType.MediaType));

			var match = ExtRegex.Match(headers.ContentDisposition.FileName);
			if (match.Groups.Count == 0)
				throw new InternalErrorException("Unknown file extension");
			
			var ext = match.Groups[match.Groups.Count - 1].Value;
			return UserPicFileNameGenerator.GenerateFileName(_userId, ext);
			//string generatedName = base.GetLocalFileName(headers).Replace("BodyPart_", "");
			//return string.Format("{0}_{1}{2}", _userId, generatedName, ext);
		}
	}
}