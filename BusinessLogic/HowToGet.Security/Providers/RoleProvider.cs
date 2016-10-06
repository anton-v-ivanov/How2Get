// The MIT License (MIT)
//
// Copyright (c) 2011 Adrian Lanning <adrian@nimblejump.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all copies or 
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Linq;
using System.Collections.Specialized;
using System.Configuration.Provider;
using HowToGet.Models.Users;
using HowToGet.Repository.Helpers;
using HowToGet.Repository.Interfaces;
using HowToGet.Repository.Repositories;
using HowToGet.Security.Helpers;
using MongoDB.Bson;

namespace HowToGet.Security.Providers
{
	/// <summary>
	/// ASP.NET Role Provider that uses MongoDB
	/// 
	/// Notes:
	///
	///   Role Collection documents consist only of an Id which is the roleName.
	///     Ex: { _id:"admin" }
	///   Roles will be stored as lowercase to prevent duplicates ("Admin" vs "admin").
	///
	/// Non-standard configuration attributes:
	///
	///   invalidUsernameCharacters - characters that are illegal in Usernames.  Default: ",%"
	///        Ex: invalidUsernameCharacters=",%&"
	///        Note: the percent character, "%", should generally always be illegal since it is used in the FindUsersBy*
	///        methods to indicate a wildcard.  This matches the behavior of the SQL Membership provider in supporting 
	///        basic SQL Like syntax, although only the "%" is supported (not "_" or "[]")
	///
	///   invalidRoleCharacters - characters that are illegal in a Role name.  Default: ",%"
	///        Ex: invalidRoleCharacters=",!%*"
	///        Note: the percent character, "%", should generally always be illegal since it is used in the FindUsersBy*
	///        methods to indicate a wildcard.  This matches the behavior of the SQL Membership provider in supporting 
	///        basic SQL Like syntax, although only the "%" is supported (not "_" or "[]")
	///
	///   roleCollectionSuffix - suffix of the collection to use for role data.  Default: "users"
	///        Ex: userCollectionSuffix="users"
	///        Note: the actual collection name used will be the combination of the ApplicationName and the roleCollectionSuffix.
	///        For example, if the ApplicationName is "/", then the default user Collection name will be "/users".
	///        This relieves us from having to include the ApplicationName in every query and also saves space in two ways:
	///          1. ApplicationName does not need to be stored with the User data
	///          2. Indexes no longer need to be composite. ie. "LowercaseUsername" rather than "ApplicationName", "LowercaseUsername"
	///
	///   userCollectionSuffix - suffix of the collection to use for Membership User data.  Default: "users"
	///        Ex: userCollectionSuffix="users"
	///
	/// </summary>
	public class RoleProvider : System.Web.Security.RoleProvider
	{
		private const string DefaultName = "MongoRoleProvider";
		private const string DefaultInvalidCharacters = ",%";
		private const int MaxRoleLength = 256;

		public override string ApplicationName { get; set; }
		private string _invalidRoleCharacters;

		private IRoleRepository _roleRepository;
		private IRoleRepository RoleRepository
		{
			get { return _roleRepository ?? (_roleRepository = new RoleRepository()); }
		}

		private IUserRepository _userRepository;
		private IUserRepository UserRepository
		{
			get { return _userRepository ?? (_userRepository = new UserRepository()); }
		}

		#region Public Methods

		public override void Initialize(string name, NameValueCollection config)
		{

			if (config == null)
				throw new ArgumentNullException("config");

			if (String.IsNullOrEmpty(name))
				name = DefaultName;

			if (String.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", Resources.RoleProvider_description);
			}
			base.Initialize(name, config);


			ApplicationName = config["applicationName"] ?? System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
			_invalidRoleCharacters = SecurityHelper.GetConfigValue(config["invalidRoleCharacters"], DefaultInvalidCharacters);
			


			// Check for unrecognized config values

			config.Remove("applicationName");
			config.Remove("connectionStringName");
			config.Remove("invalidRoleCharacters");

			if (config.Count > 0)
			{
				string key = config.GetKey(0);
				if (!String.IsNullOrEmpty(key))
					throw new ProviderException(String.Format(Resources.Provider_unrecognized_attribute, key));
			}
		}


		public override void AddUsersToRoles(string[] userIds, string[] roleNames)
		{
			SecUtility.CheckArrayParameter(ref roleNames, true, true, _invalidRoleCharacters, MaxRoleLength, "roleNames");

			var roles = roleNames.Select(role => role.ToLowerInvariant()).ToList();
			foreach (var role in roles)
				CreateRole(role);

			try
			{
				UserRepository.UpdateUserRoles(userIds.Select(ObjectId.Parse).ToArray(), roles);
			}
			catch (Exception exception)
			{
				throw new ProviderException(exception.Message);
			}
		}

		public override void CreateRole(string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, _invalidRoleCharacters, MaxRoleLength, "roleName");

			var role = new UserRole(roleName.ToLowerInvariant());

			try
			{
				RoleRepository.CreateRole(role);
			}
			catch (Exception exception)
			{
				throw new ProviderException(String.Format("Could not create role '{0}'. Reason: {1}", roleName, exception.Message));
			}
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			SecUtility.CheckParameter(ref roleName, true, true, _invalidRoleCharacters, MaxRoleLength, "roleName");

			if (throwOnPopulatedRole && UserRepository.GetUsersInRole(roleName).Any())
			{
				throw new ProviderException(Resources.Role_is_not_empty);
			}
			
			RoleRepository.DeleteRole(roleName);
			return true;
		}

		public override string[] FindUsersInRole(string roleName, string userId)
		{
			var id = MongoHelper.GetObjectIdFromString(userId, "{0} is not valid userId");
			var users = UserRepository.FindUsersInRole(roleName, id);
			return users.Select(u => u.Id).ToArray();
		}


		public override string[] GetAllRoles()
		{
			return RoleRepository.GetAllRoles().Select(s => s.RoleName).ToArray();
		}

		public override string[] GetRolesForUser(string userId)
		{
			var id = MongoHelper.GetObjectIdFromString(userId, "{0} is not valid userId");
			var user = UserRepository.GetUser(id, false);
			if (user == null)
				throw new ProviderException(string.Format("User with Id = {0} was not found", userId));

			return user.Roles.ToArray();
		}

		public override string[] GetUsersInRole(string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, _invalidRoleCharacters, MaxRoleLength, "roleName");
			var users = UserRepository.GetUsersInRole(roleName);
			return users.Select(u => u.Id).ToArray();
		}

		public override bool IsUserInRole(string userId, string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, null, MaxRoleLength, "roleName");
			
			var id = MongoHelper.GetObjectIdFromString(userId, "{0} is not valid userId");
			var user = UserRepository.GetUser(id, false);
			if(user == null)
				throw new ProviderException(string.Format("User with Id = {0} was not found", userId));
			
			return user.Roles.Contains(roleName.ToLowerInvariant());
		}

		public override void RemoveUsersFromRoles(string[] userIds, string[] roleNames)
		{
			SecUtility.CheckArrayParameter(ref roleNames, true, true, _invalidRoleCharacters, MaxRoleLength, "roleNames");

			var roles = roleNames.Select(role => role.ToLowerInvariant());
			var users = userIds.Select(userId => MongoHelper.GetObjectIdFromString(userId, "{0} is not valid userId"));

			try
			{
				UserRepository.RemoveUsersFromRoles(users, roles);
			}
			catch (Exception exception)
			{
				throw new ProviderException(exception.Message);
			}
		}

		public override bool RoleExists(string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, null, MaxRoleLength, "roleName");
			return RoleRepository.RoleExists(roleName);
		}


		#endregion
	}
}
