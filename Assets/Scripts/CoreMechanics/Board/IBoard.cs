using CoreMechanics.Units;
using CoreMechanics.Utilities;

namespace CoreMechanics.Board
{
	public interface IBoard
	{
		void AddUnit(Unit unit, Vec2Int position);
		void RemoveUnit(Unit unit);
		Vec2Int[] GetPossiblePositions(Unit unit);
		bool MoveUnit(Unit performer, Vec2Int position);
		Unit GetUnitInPosition(Vec2Int position);
	}
}