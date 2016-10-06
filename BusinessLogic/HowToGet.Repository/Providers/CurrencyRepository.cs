using System.Collections.Generic;
using System.Linq;
using BLToolkit.Data;
using HowToGet.Models.Dictionaries;
using HowToGet.Repository.Interfaces;

namespace HowToGet.Repository.Providers
{
    public class CurrencyRepository : ICurrencyRepository
    {
        public IEnumerable<Currency> GetAll()
        {
            using (var db = new DbManager())
            {
                return db
                    .SetSpCommand("GetAllCurrencies")
                    .ExecuteList<Currency>();
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