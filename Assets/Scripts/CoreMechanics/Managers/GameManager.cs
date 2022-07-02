using System;
using System.Collections.Generic;
using CoreMechanics.Actions;
using CoreMechanics.Boards;
using CoreMechanics.Teams;
using CoreMechanics.Units;

namespace CoreMechanics.Managers
{
	// The game has steps:
	// - team creation
	// - units creation
	// - game start
	// - turns until a team wins or a draw
	// - results
	// - reset

	public sealed class GameManager
	{
		public event Action<ITeam> GameCompleted;

		private readonly IConfigsManager mConfigsManager;
		private readonly List<ITeam> mTeams = new();

		private TurnManager mTurnManager;
		private ActionManager mActionManager;
		private Board mBoard;

		public ITeam[] Teams => mTeams.ToArray();

		public GameManager(IConfigsManager configsManager)
		{
			mConfigsManager = configsManager;
		}

		public void RegisterTeam(string name)
		{
			var team = new Team(name);
			mTeams.Add(team);
		}

		public void AddUnitsToTeam(ITeam team, IEnumerable<UnitTypePositionPair> unitsData)
		{
			var units = new List<Unit>();
			foreach (var data in unitsData)
			{
				var unit = CreateUnit(data);
				mBoard.AddUnit(unit, data.Position);
				units.Add(unit);
			}

			team.AddUnits(units);
		}

		public void StartGame()
		{
			foreach (var team in mTeams) team.UnitRequestedAction += OnUnitRequestedAction;

			mTurnManager = new TurnManager(Teams);
			mTurnManager.GameCompleted += OnGameCompleted;

			mBoard = new Board(mConfigsManager.BoardConfig);
			mActionManager = new ActionManager(mConfigsManager.ActionConfigs, mBoard);
		}

		public void ResetGame()
		{
			foreach (var team in mTeams)
			{
				team.UnitRequestedAction -= OnUnitRequestedAction;
				RemoveUnitsFromTeam(team);
			}
			mTeams.Clear();

			mTurnManager.GameCompleted -= OnGameCompleted;
			mTurnManager = null;

			mBoard = null;
			mActionManager = null;
		}

		private void OnGameCompleted(ITeam winner)
		{
			GameCompleted?.Invoke(winner);
		}

		private void OnUnitRequestedAction(Unit sender, ActionType actionType, object extraParameters = null)
		{
			mActionManager.PerformAction(actionType, sender, extraParameters);
		}

		private Unit CreateUnit(UnitTypePositionPair data)
		{
			var config = mConfigsManager.GetUnitConfig(data.UnitType);
			return new Unit(config);
		}

		private void RemoveUnitsFromTeam(ITeam team)
		{
			foreach (var unit in team.Units) mBoard.RemoveUnit(unit);

			team.RemoveAllUnits();
		}
	}
}