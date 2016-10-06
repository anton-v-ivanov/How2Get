using MongoDB.Driver;

namespace Sql2MongoMigration
{
	public static class MongoHelper
	{
		public static MongoClient Client { get; private set; }
		
		public static MongoDatabase Database { get; private set; }

		static MongoHelper()
		{
			Client = new MongoClient("mongodb://localhost/howtoget");
			var server = Client.GetServer();
			Database = server.GetDatabase("howtoget");
		}
	}
}