using System;
using System.Collections.Generic;
using System.Linq;
using CoreMechanics.Units;
using CoreMechanics.Utilities;

namespace CoreMechanics.Managers
{
	public static class AttackHandler
	{
		public static void ResolveClash(Unit attacker, Unit defender)
		{
			Handle(attacker, defender);
			if (!defender.ReturnAttack || defender.Dead) return;
			Handle(defender, attacker);
		}

		public static AttackPosition[] CreateAttackPositions(
			Vec2Int position,
			Orientation orientation,
			IEnumerable<AttackPosition> attackPositions,
			IEnumerable<AttackPosition> defaultPositions)
		{
			return attackPositions
				.Zip(defaultPositions, (c, d) =>
				{
					c.Position = RotateVectorToOrientation(d.Position, orientation) + position;
					return c;
				})
				.ToArray();
		}

		private static void Handle(Unit attacker, Unit defender)
		{
			for (var i = 0; i < attacker.AttackPositions.Length; i++)
			{
				if (attacker.AttackPositions[i].Position != defender.Position) continue;
				var damage = Math.Min(attacker.AttackPositions[i].Points, attacker.GetDamageForType(defender.Type));
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