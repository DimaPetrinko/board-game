using System;
using System.Collections.Generic;
using CoreMechanics.Units;

namespace CoreMechanics.Teams
{
	public interface ITeam
	{
		event Action TurnStarted;
		event Action TurnCompleted;
		event UnitActionRequest UnitRequestedAction;

		string Name { get; }
		bool IsDefeated { get; }
		Unit[] Units { get; }

		void AddUnits(IEnumerable<Unit> units);
		void RemoveAllUnits();

		void StartTurn();
		void EndTurn();
	}
}