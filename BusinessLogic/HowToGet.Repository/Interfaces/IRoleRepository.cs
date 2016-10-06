using System.Collections.Generic;
using HowToGet.Models.Users;

namespace HowToGet.Repository.Interfaces
{
	public interface IRoleRepository
	{
		IEnumerable<UserRole> GetAllRoles();
		
		bool RoleExists(string roleName);
		
		void CreateRole(UserRole role);
		
		void DeleteRole(string role);
	}
}