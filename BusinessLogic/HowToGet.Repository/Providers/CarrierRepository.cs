using System.Collections.Generic;
using BLToolkit.Data;
using HowToGet.Repository.Interfaces;
using HowToGet.Models.Dictionaries;

namespace HowToGet.Repository.Providers
{
    public class CarrierRepository : ICarrierRepository
    {
        public IEnumerable<Carrier> GetAll()
        {
            using (var db = new DbManager())
            {
                return db
                    .SetSpCommand("GetAllCarriers")
                    .ExecuteList<Carrier>();
            }
        }

        public Carrier GetById(int id)
        {
            using (var db = new DbManager())
            {
                return db
                    .SetSpCommand("GetCarrierById", db.Parameter("@id", id))
                    .ExecuteObject<Carrier>();
            }
        }
    }
}