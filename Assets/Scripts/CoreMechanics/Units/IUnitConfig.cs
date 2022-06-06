using System.Collections.ObjectModel;
using UnityEngine;

namespace CoreMechanics.Units
{
	public struct AttackPosition
	{
		public int PatternIndex;
		public Vector2Int Position;

		public AttackPosition(int index, Vector2Int position)
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