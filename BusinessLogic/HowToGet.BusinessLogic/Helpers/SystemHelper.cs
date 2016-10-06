using System.Collections.Generic;
using HowToGet.Notifications.Providers;
using HowToGet.Repository.Helpers;
using MongoDB.Bson;

namespace HowToGet.BusinessLogic.Helpers
{
	public class SystemHelper
	{
		public static void EnsureMongoIndexes()
		{
			var indexes = new Dictionary<string, string[]>
				              {
					              {"carriers", new[]{"ln", "ci"}},
								  {"cities", new[]{"n", "anl"}},
								  {"currencies", new[]{"ci", "CurrencyCode"}},
								  {"currencyRates", new[]{"cf", "ct"}},
								  {"routes", new[]{"rp", "rp.o", "r", "u"}},
								  {"users", new[]{"lemail", "lname"}},
								  {"tokens", new[]{"u", "t"}},
								  {"extUsers", new[]{"extId"}},
								  {"findedRoutes", new[]{"o", "d"}},
								  {"bonuses", new[]{"u", "t"}},
								  {"notifications", new[]{"u", "r"}},
								  {"invites", new[]{"i", "g", "s"}},
								  {"userLocations", new[]{"uid"}},
								  {"foursq", new[]{"fsqid", "uid"}},
								  {"routeAnnouncements", new[]{"o", "d", "e", "u"}},
				              };
			MongoHelper.EnsureIndexes(indexes);
		}

		public static ObjectId GetObjectIdFromString(string id, string errorMessage)
		{
			return MongoHelper.GetObjectIdFromString(id, errorMessage);
		}

		public static void StartFailedEmailsQueue()
		{
			FailedEmailsProcessor.StartEmailQueue();
		}
	}
}