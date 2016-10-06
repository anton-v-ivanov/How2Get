using System.Collections.Generic;
using System.Data;
using System.Linq;
using BLToolkit.Data;
using BLToolkit.Mapping;
using HowToGet.Models.Dictionaries;

namespace Sql2MongoMigration.Repositories
{
    internal class CurrencyRepository
    {
        public IEnumerable<Currency> GetAll()
        {
            using (var db = new DbManager())
            {
	            var table = db
		            //.SetSpCommand("GetAllCurrencies")
		            .SetCommand(@"SELECT DiSTINCT c1.*, c2.id AS CountryId FROM Currencies c1
	INNER JOIN Countries c2 ON c2.currencyId = c1.id
GROUP BY c1.id, c1.currencyCode, c1.currencyName, c2.id")
		            .ExecuteDataTable();
	            
				var result = new List<Currency>();
	            int prevId = 0;
	            int index = 0;
				foreach (DataRow row in table.Rows)
				{
					var currency = Map.DataRowToObject<Currency>(row);
					currency.CountryIds = new List<int>
						                      {
							                      (int)row["CountryId"]
						                      };
					if (prevId == 0)
					{
						result.Add(currency);
						index++;						
						prevId = currency.Id;
						continue;
					}

					if (currency.Id != prevId)
					{
						prevId = currency.Id;
						result.Add(currency);
						index++;
					}
					else
					{
						result[index - 1].CountryIds.Add((int)row["CountryId"]);
					}
				}
	            return result;
            }
        }

        public Currency GetById(int id)
        {
            using (var db = new DbManager())
            {
                return db
                    .SetSpCommand("GetCurrencyById", db.Parameter("@id", id))
                    .ExecuteObject<Currency>();
            }
        }

        public IEnumerable<Currency> GetForCountry(int countryId)
        {
            using (var db = new DbManager())
            {
                return db
                    .SetSpCommand("GetCurrenciesForCountry", db.Parameter("@countryId", countryId))
                    .ExecuteList<Currency>().Distinct();
            }
        }
    }
}