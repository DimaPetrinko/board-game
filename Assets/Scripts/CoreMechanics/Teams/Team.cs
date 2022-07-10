using System;
using System.Collections.Generic;
using System.Linq;
using CoreMechanics.Actions;
using CoreMechanics.Units;

namespace CoreMechanics.Teams
{
	public sealed class Team : ITeam
	{
		public event Action TurnStarted;
		public event Action TurnCompleted;
		public event UnitActionRequest UnitRequestedAction;
		public string Name { get; }
		public bool IsDefeated => Units.All(u => u.Dead);

		public Unit[] Units { get; private set; } = Array.Empty<Unit>();

		public Team(string name)
		{
			Name = name;
		}

		public void AddUnits(IEnumerable<Unit> units)
		{
			Units = units.ToArray();
			foreach (var unit in Units) unit.ActionRequested += OnUnitActionRequested;
		}

		public void RemoveAllUnits()
		{
			foreach (var unit in Units) unit.ActionRequested -= OnUnitActionRequested;
			Units = Array.Empty<Unit>();
		}

		private void OnUnitActionRequested(Unit sender, ActionType actionType, object extraParameters)
		{
			UnitRequestedAction?.Invoke(sender, actionType, extraParameters);
		}

		public void StartTurn()
		{
			foreach (var unit in Units) unit.ResetActionPoints();

			TurnStarted?.Invoke();
		}

		public void EndTurn()
		{
			TurnCompleted?.Invoke();
		}
	}
}