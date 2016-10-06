using System;
using HowToGet.Models.Analytics;
using HowToGet.Repository.Interfaces;

namespace HowToGet.Bonuses.BonusDefinitions
{
	public class RegistrationBonus: IBonus
	{
		private readonly IUserRepository _userRepository;
		private readonly int _usersLimit;

		public Type SourceActionType { get { return typeof(RegisterAction); } }

		public RegistrationBonus(IUserRepository userRepository, int usersLimit)
		{
			_userRepository = userRepository;
			_usersLimit = usersLimit;
		}

		public bool IsValid(ActionBase userAction)
		{
			var registerAction = userAction as RegisterAction;
			if (registerAction == null)
				return false;

			var totalNumberOfUsers = _userRepository.GetNumberOfUsers();
			return (totalNumberOfUsers < _usersLimit);
		}
	}
}