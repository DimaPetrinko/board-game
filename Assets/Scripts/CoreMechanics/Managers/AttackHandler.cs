using System;
using System.Collections.Generic;
using System.Linq;
using CoreMechanics.Units;
using CoreMechanics.Utilities;

namespace CoreMechanics.Managers
{
	public class AttackHandler
	{
		public void ResolveClash(Unit firstAttacker, Unit secondAttacker)
		{
			Handle(firstAttacker, secondAttacker);
			if (secondAttacker.Dead) return;
			Handle(secondAttacker, firstAttacker);
		}

		public static AttackPosition[] CreateAttackPositions(
			Vec2Int position,
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

		private static void Handle(Unit attacker, Unit defender)
		{
			for (var i = 0; i < attacker.AttackPositions.Length; i++)
			{
				if (attacker.AttackPositions[i].Position != defender.Position) continue;
				var damage = Math.Min(attacker.AttackPoints[i], attacker.GetDamageForType(defender.Type));
				defender.Health -= damage;
			}
		}

		private static Vec2Int RotateVectorToOrientation(Vec2Int input, Orientation orientation)
		{
			var output = input;
			for (var i = 0; i < (int)orientation; i++) output = RotateVector(output);
			return output;
		}

		private static Vec2Int RotateVector(Vec2Int input)
		{
			return new Vec2Int(input.y, -input.x);
		}
	}
}