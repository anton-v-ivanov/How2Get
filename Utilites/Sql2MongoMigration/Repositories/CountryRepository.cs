using BLToolkit.Data;
using HowToGet.Models.Dictionaries;
using MongoDB.Driver.Builders;

namespace Sql2MongoMigration.Repositories
{
	public class CountryRepository
	{
		public static string GetNameByIdFromSql(int id)
		{
			using (var db = new DbManager())
			{
				return db
					.SetCommand("SELECT NAME FROM Countries WHERE id = @id", db.Parameter("@id", id))
					.ExecuteScalar<string>();
			}
		}

		public static string GetCountryIdFromMongo(string name)
		{
			var collection = MongoHelper.Database.GetCollection<Country>("countries");
			var query = Query.EQ("n", name);
			return collection.FindOne(query).Id;
		}
	}
}