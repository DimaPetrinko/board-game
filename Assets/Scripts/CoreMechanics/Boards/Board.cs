using System.Collections.Generic;
using System.Linq;
using CoreMechanics.Units;
using CoreMechanics.Utilities;

namespace CoreMechanics.Boards
{
	public sealed class Board : IBoard
	{
		public static readonly Vec2Int[] Mask =
		{
			new(-1, 1), new(0, 1), new(1, 1),
			new(-1, 0), new(0, 0), new(1, 0),
			new(-1, -1), new(0, -1), new(1, -1)
		};

		private readonly IBoardConfig mConfig;
		private readonly HashSet<Unit> mUnits;

		public Board(IBoardConfig config)
		{
			mConfig = config;
			mUnits = new HashSet<Unit>();
		}

		public void AddUnit(Unit unit, Vec2Int position)
		{
			if (PositionOccupied(position) || mUnits.Contains(unit)) return;
			mUnits.Add(unit);
			unit.Position = position;
			unit.Died += OnUnitDied;
		}

		public void RemoveUnit(Unit unit)
		{
			if (!mUnits.Contains(unit)) return;
			mUnits.Remove(unit);
			unit.Died -= OnUnitDied;
		}

		public Vec2Int[] GetPossiblePositions(Unit unit)
		{
			return Mask
				.Select(p => p + unit.Position)
				.Where(p => p.x >= 0
					&& p.y >= 0
					&& p.x <= mConfig.BoardSize.x
					&& p.y <= mConfig.BoardSize.y
					&& p != unit.Position)
				.ToArray();
		}

		public bool MoveUnit(Unit performer, Vec2Int position)
		{
			if (PositionOccupied(position) || !GetPossiblePositions(performer).Contains(position)) return false;
			performer.Position = position;
			return true;
		}

		public Unit GetUnitInPosition(Vec2Int position)
		{
			return mUnits.FirstOrDefault(u => u.Position == position);
		}

		private bool PositionOccupied(Vec2Int position)
		{
			return mUnits.Any(u => u.Position == position);
		}

		private void OnUnitDied(Unit sender)
		{
			RemoveUnit(sender);
		}
	}
}