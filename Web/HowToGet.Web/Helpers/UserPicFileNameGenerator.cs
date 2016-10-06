using System;
using HowToGet.Models.Exceptions;

namespace HowToGet.Web.Helpers
{
	public class UserPicFileNameGenerator
	{
		public static string GenerateFileName(string userId, string fileExtension)
		{
			if(string.IsNullOrEmpty(userId))
				throw new InvalidObjectIdException("Failed to generate file name for user picture. UserId is empty");

			var generatedName = Guid.NewGuid().ToString();
			if (!fileExtension.StartsWith("."))
				fileExtension = "." + fileExtension;

			return string.Format("{0}_{1}{2}", userId, generatedName, fileExtension);
		}
	}
}