using HowToGet.Repository.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;

namespace HowToGet.Tests
{
	[TestClass]
	public class MongoCarrierRepositoryTests
	{
		[TestMethod]
		public void GetAllTest()
		{
			var repo = new CarrierRepository();
			var result = repo.GetAll();
		}

		[TestMethod]
		public void GetByIdTest()
		{
			var repo = new CarrierRepository();
			var result = repo.GetById(ObjectId.Parse("1"));
		}
	}
}
