using System.Linq;
using CoreMechanics.Units;
using UnityEngine;

namespace CoreMechanics
{
	public class AttackHandler
	{
		private static readonly Vector2Int[] sMask =
		{
			new(-1, 1), new(0, 1), new(1, 1),
			new(-1, 0), new(0, 0), new(1, 0),
			new(-1, -1), new(0, -1), new(1, -1)
		};

		public void ResolveClash(Unit firstAttacker, Unit secondAttacker)
		{
			Handle(firstAttacker, secondAttacker);
			if (secondAttacker.Dead) return;
			Handle(secondAttacker, firstAttacker);
		}

		public void Handle(Unit attacker, Unit defender)
		{
			if (attacker.AttackPositions.Contains(defender.Position))
			{
				defender.Health -= attacker.GetDamageForType(defender.Type);
			}
		}

		public static Vector2Int[] CreateAttackPositions(
			Vector2Int position,
			Orientation orientation,
			int[] attackPattern)
		{
			var result = new Vector2Int[9];
			for (var i = 0; i < 9; i++)
			{
				var rotatedIndex = RotateIndex(i, orientation);
				var offset = sMask[i] * attackPattern[rotatedIndex];
				switch (orientation)
				{
					case Orientation.North:
					case Orientation.South:
						offset.x = Mathf.Clamp(offset.x, -1, 1);
						break;
					case Orientation.East:
					case Orientation.West:
						offset.y = Mathf.Clamp(offset.y, -1, 1);
						break;
				}
				result[i] = position + offset;
			}

			return result;
		}

		private static int RotateIndex(int index, Orientation orientation)
		{
			var rotated = index;

			for (var i = 0; i < (int)orientation; i++)
			{
				var x = (8 - rotated) % 3;
				var y = (int)((8 - rotated) / 9f * 3);
				rotated = 2 - y + 3 * x;
			}

			return rotated;
		}
	}
}