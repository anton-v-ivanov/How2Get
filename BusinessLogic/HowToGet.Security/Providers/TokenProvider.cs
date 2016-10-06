using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Web;
using System.Web.Security;
using HowToGet.Models.Exceptions;
using HowToGet.Models.Security;
using HowToGet.Models.Users;
using HowToGet.Repository.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace HowToGet.Security.Providers
{
	public class TokenProvider
	{
		private const string AuthorizationHeaderName = "Authorization";

		private static List<AuthToken> _tokens;
		private static List<AuthToken> Tokens
		{
			get { return _tokens ?? (_tokens = new List<AuthToken>()); }
		}

		private static readonly object SyncObj = new object();

		public static string GenerateAuthToken(MembershipUser user)
		{
			if(user == null || user.ProviderUserKey == null)
				throw new SecurityException("UserNotFound");

			var userId = user.ProviderUserKey.ToString();
			return GenerateAuthToken(userId);
		}

		public static string GenerateAuthToken(string userId)
		{
			var token = ObjectId.GenerateNewId().ToString();
			var authToken = new AuthToken(userId, token);
			lock (SyncObj)
			{
				Tokens.Add(authToken);
			}

			MongoHelper.Database.GetCollection<AuthToken>("tokens").Insert(authToken);

			return token;
		}

		public static bool TryGetUserIdFromAuthHeader(HttpRequestMessage message, out string userId)
		{
			if (message != null && message.Headers != null)
			{
				if (message.Headers.Contains(AuthorizationHeaderName))
				{
					var headerValue = message.Headers.GetValues(AuthorizationHeaderName).First();
					userId = GetUserIdFromTokenValue(headerValue);
					return !string.IsNullOrEmpty(userId);
				}
			}
			userId = null;
			return false;
		}

		public static bool TryGetUserIdFromAuthHeader(HttpRequest request, out string userId)
		{
			var header = request.Headers.GetValues(AuthorizationHeaderName);
			if (header != null)
			{
				userId = GetUserIdFromTokenValue(header.FirstOrDefault());
				return !string.IsNullOrEmpty(userId);
			}
			userId = null;
			return false;
		}

		public static string GetUserIdFromTokenValue(string headerValue)
		{
			var userToken = Tokens.FirstOrDefault(s => s.Token == headerValue);
			if (userToken == null)
			{
				ObjectId objectId;
				if (!ObjectId.TryParse(headerValue, out objectId))
					return null;

				var query = Query.EQ("t", objectId);
				userToken = MongoHelper.Database.GetCollection<AuthToken>("tokens").FindOne(query);
				if (userToken != null)
				{
					lock (SyncObj)
					{
						Tokens.Add(userToken);
					}
					return userToken.UserId;
				}
			}
			else
			{
				return userToken.UserId;
			}
			return null;
		}

		public static void RemoveToken(string userId, string token)
		{
			var query = Query.And(
				Query.EQ("t", ObjectId.Parse(token)),
				Query.EQ("u", ObjectId.Parse(userId)));
			MongoHelper.Database.GetCollection<AuthToken>("tokens").Remove(query, RemoveFlags.Single);
			lock (SyncObj)
			{
				Tokens.RemoveAll(s => s.UserId == userId && s.Token == token);
			}
		}
	}
}