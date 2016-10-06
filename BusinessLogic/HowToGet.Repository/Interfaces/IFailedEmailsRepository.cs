using System.Collections.Generic;
using HowToGet.Models.Users;

namespace HowToGet.Repository.Interfaces
{
	public interface IFailedEmailsRepository
	{
		void InsertFailedEmail(UserEmail email);

		IEnumerable<UserEmail> GetAll();
		
		bool IsEmailExists(UserEmail email);
		
		void DeleteEmail(UserEmail email);
	}
}