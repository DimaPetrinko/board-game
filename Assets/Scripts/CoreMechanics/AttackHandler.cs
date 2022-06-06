using System.Collections.Generic;
using System.Linq;
using CoreMechanics.Units;
using UnityEngine;

namespace CoreMechanics
{
	public class AttackHandler
	{
		public void ResolveClash(Unit firstAttacker, Unit secondAttacker)
		{
			Handle(firstAttacker, secondAttacker);
			if (secondAttacker.Dead) return;
			Handle(secondAttacker, firstAttacker);
		}

		public void Handle(Unit attacker, Unit defender)
		{
			for (var i = 0; i < attacker.AttackPositions.Length; i++)
			{
				if (attacker.AttackPositions[i].Position != defender.Position) continue;
				var damage = Mathf.Min(attacker.AttackPoints[i], attacker.GetDamageForType(defender.Type));
				defender.Health -= damage;
			}
		}

		public static AttackPosition[] CreateAttackPositions(
			Vector2Int position,
			Orientation orientation,
			IEnumerable<AttackPosition> attackPositions)
		{
			return attackPositions
				.Select(p =>
				{
					p.Position = RotateVectorToOrientation(p.Position, orientation) + position;
					return p;
				})
				.ToArray();
		}

		private static Vector2Int RotateVectorToOrientation(Vector2Int input, Orientation orientation)
		{
			var output = input;
			for (var i = 0; i < (int)orientation; i++) output = RotateVector(output);
			return output;
		}

		private static Vector2Int RotateVector(Vector2Int input)
		{
			return new Vector2Int(input.y, -input.x);
		}
	}
}