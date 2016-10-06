using System;
using HowToGet.Models.Analytics;

namespace HowToGet.Bonuses.BonusDefinitions
{
	public interface IBonus
	{
		Type SourceActionType { get; }

		bool IsValid(ActionBase userAction);
	}
}