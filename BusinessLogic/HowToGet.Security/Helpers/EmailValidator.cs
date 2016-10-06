using System.Text.RegularExpressions;

namespace HowToGet.Security.Helpers
{
	public static class EmailValidator
	{
		/// <summary>
		/// Regular expression, which is used to validate an E-Mail address.
		/// </summary>
		private const string MatchEmailPattern =
				  @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
				+ @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
					[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
				+ @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
					[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
				+ @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

		private static readonly Regex EmailRegex = new Regex(MatchEmailPattern, RegexOptions.Compiled);

		/// <summary>
		/// Checks whether the given Email-Parameter is a valid E-Mail address.
		/// </summary>
		/// <param name="email">Parameter-string that contains an E-Mail address.</param>
		/// <returns>True, when Parameter-string is not null and 
		/// contains a valid E-Mail address;
		/// otherwise false.</returns>
		public static bool Validate(string email)
		{
			return !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email);
		}
	}
}