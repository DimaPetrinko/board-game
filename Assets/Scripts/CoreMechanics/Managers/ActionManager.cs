using System.Collections.Generic;
using System.Linq;
using CoreMechanics.Actions;
using CoreMechanics.Board;
using CoreMechanics.Units;

namespace CoreMechanics.Managers
{
	public class ActionManager
	{
		// TODO: improve sending the parameters
		private delegate bool UnitAction(Unit performer, object extraParameters, IActionConfig config);

		private readonly IBoard mBoard;
		private readonly Dictionary<ActionType, IActionConfig> mActionConfigs;
		private readonly Dictionary<ActionType, UnitAction> mActionMethods;

		public ActionManager(
			IEnumerable<IActionConfig> configs,
			IBoard board)
		{
			mBoard = board;
			mActionConfigs = configs.ToDictionary(c => c.Type, c => c);
			mActionMethods = new Dictionary<ActionType, UnitAction>
			{
				{ActionType.Attack, Attack},
				{ActionType.AttackFocus, AttackFocus},
				{ActionType.Heal, Heal},
				{ActionType.Move, Move},
				{ActionType.Rotate, Rotate},
			};
		}

		public void PerformAction(ActionType type, Unit performer, object extraParameters = null)
		{
			if (!mActionConfigs.TryGetValue(type, out var config)) return;
			if (!HasEnoughPoints(performer, config)) return;
			if (mActionMethods[type](performer, extraParameters, config))
				UsePoints(performer, config);
		}

		private bool Attack(Unit performer, object extraParameters, IActionConfig config)
		{
			var defenders = performer.AttackPositions
				.Where(p => p.Points > 0)
				.OrderByDescending(p => p.Points)
				.Select(p => p.Position)
				.Select(mBoard.GetUnitInPosition)
				.Where(u => u != null)
				.ToArray();

			if (defenders.Length == 0) return false;
			foreach (var defender in defenders)
			{
				AttackHandler.ResolveClash(performer, defender);
				if (performer.Dead) break;
			}
			return true;
		}

		private bool AttackFocus(Unit performer, object extraParameters, IActionConfig config)
		{
			var focusParameters = (FocusParameters)extraParameters;
			var status = false;
			foreach (var parameters in focusParameters.Points
				.OrderBy(p => p.Points - performer.AttackPositions[p.PositionIndex].Points))
			{
				if (parameters.PositionIndex >= performer.AttackPositions.Length
					|| parameters.Points == performer.AttackPositions[parameters.PositionIndex].Points)
					continue;
				performer.AssignAttackPoint(parameters.PositionIndex, parameters.Points);
				status = true;
			}

			return status;
		}

		private bool Heal(Unit performer, object extraParameters, IActionConfig config)
		{
			var healConfig = (IHealConfig)config;
			if (performer.Healthy) return false;
			performer.Health += healConfig.HealAmount;
			return true;
		}

		private bool Move(Unit performer, object extraParameters, IActionConfig config)
		{
			var moveParameters = (MoveParameters)extraParameters;
			return mBoard.MoveUnit(performer, moveParameters.Position);
		}

		private bool Rotate(Unit performer, object extraParameters, IActionConfig config)
		{
			var rotateParameters = (RotateParameters)extraParameters;
			if (performer.Orientation == rotateParameters.Orientation) return false;
			performer.Orientation = rotateParameters.Orientation;
			return true;
		}

		private static bool HasEnoughPoints(Unit performer, IActionConfig config)
		{
			return performer.ActionPoints >= config.Cost;
		}

		private static void UsePoints(Unit performer, IActionConfig config)
		{
			performer.ActionPoints -= config.Cost;
		}
	}
}