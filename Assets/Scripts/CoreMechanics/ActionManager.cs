using System.Collections.Generic;
using System.Linq;
using CoreMechanics.Actions;
using CoreMechanics.Units;
using UnityEngine;

namespace CoreMechanics
{
	// TODO: development is ongoing
	public interface IBoard
	{
		void MoveUnit(Unit performer, Vector2Int position);
	}

	public class FakeBoard : IBoard
	{
		public void MoveUnit(Unit performer, Vector2Int position)
		{
			// check if can move there
			// if so - move
			performer.Position = position;
			// else - do nothing
		}
	}

	public class ActionManager
	{
		// TODO: improve sending the parameters
		private delegate void UnitAction(Unit performer, object extraParameters, IActionConfig config);

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
			var config = mActionConfigs[type];
			if (!HasEnoughPoints(performer, config)) return;
			mActionMethods[type](performer, extraParameters, config);
			UsePoints(performer, config);
		}

		private void Attack(Unit performer, object extraParameters, IActionConfig config)
		{
			// get receivers in attack position from the board
		}

		private void AttackFocus(Unit performer, object extraParameters, IActionConfig config)
		{
			var focusParameters = (FocusParameters[])extraParameters;
			foreach (var p in focusParameters) performer.AssignAttackPoint(p.PositionIndex, p.Points);
		}

		private void Heal(Unit performer, object extraParameters, IActionConfig config)
		{
			var healConfig = (IHealConfig)config;
			performer.Health += healConfig.HealAmount;
		}

		private void Move(Unit performer, object extraParameters, IActionConfig config)
		{
			var moveParameters = (MoveParameters)extraParameters;
			mBoard.MoveUnit(performer, moveParameters.Position);
		}

		private void Rotate(Unit performer, object extraParameters, IActionConfig config)
		{
			var rotateParameters = (RotateParameters)extraParameters;
			performer.Orientation = rotateParameters.Orientation;
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