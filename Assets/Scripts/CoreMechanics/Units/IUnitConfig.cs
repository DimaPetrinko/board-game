using System.Collections.ObjectModel;
using CoreMechanics.Utilities;

namespace CoreMechanics.Units
{
	public struct AttackPosition
	{
		public readonly int PatternIndex;
		public Vec2Int Position;

		public AttackPosition(int index, Vec2Int position)
		{
			PatternIndex = index;
			Position = position;
		}
	}

	public interface IUnitConfig
	{
		int Health { get; }
		int ActionPoints { get; }
		int AttackPoints { get; }
		UnitType Type { get; }
		ReadOnlyDictionary<UnitType, int> DamageByType { get; }
		AttackPosition[] AttackPositions { get; }
	}
}