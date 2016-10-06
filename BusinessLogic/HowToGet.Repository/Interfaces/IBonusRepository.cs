using System.Collections.Generic;
using HowToGet.Models.Bonuses;
using MongoDB.Bson;

namespace HowToGet.Repository.Interfaces
{
	public interface IBonusRepository
	{
		void AddBonus(Bonus bonus);

		void Remove(ObjectId bonus);

		IEnumerable<Bonus> GetBonuses(ObjectId userId);
	}
}