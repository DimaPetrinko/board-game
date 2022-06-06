using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoreMechanics.Units;
using CoreMechanics.Utilities;
using UnityEngine;

namespace Frontend.Configs
{
	[CreateAssetMenu(menuName = "Configs/Unit", fileName = "Unit", order = 0)]
	public sealed class UnitConfig : ScriptableObject, IUnitConfig
	{
		[Serializable]
		public struct TypeDamage
		{
			public UnitType Type;
			public int Damage;
		}

		[SerializeField] private int m_Health;
		[SerializeField] private int m_ActionPoints;
		[SerializeField] private int m_AttackPoints;
		[SerializeField] private UnitType m_Type;
		[SerializeField] private TypeDamage[] m_DamageByType;
		[SerializeField] [TextArea] private string m_AttackPattern;

		public int Health => m_Health;
		public int ActionPoints => m_ActionPoints;
		public int AttackPoints => m_AttackPoints;
		public UnitType Type => m_Type;
		public ReadOnlyDictionary<UnitType, int> DamageByType { get; private set; }
		public AttackPosition[] AttackPositions { get; private set; }

		private static readonly Vec2Int[] sMask =
		{
			new(-1, 1), new(0, 1), new(1, 1),
			new(-1, 0), new(0, 0), new(1, 0),
			new(-1, -1), new(0, -1), new(1, -1)
		};

		private void OnValidate()
		{
			DamageByType = new ReadOnlyDictionary<UnitType, int>(m_DamageByType
				.ToDictionary(p => p.Type, p => p.Damage));

			ValidateAttackPoints();
		}

		private void ValidateAttackPoints()
		{
			var pattern = m_AttackPattern
				.Where(ch => !ch.Equals('\n'))
				.Select(ch => int.Parse(ch.ToString()))
				.ToArray();
			var points = new List<AttackPosition>();
			for (var i = 0; i < 9; i++)
			{
				var offset = sMask[i] * pattern[i];
				offset.x = Mathf.Clamp(offset.x, -1, 1);
				if (pattern[i] > 0) points.Add(new AttackPosition(i, offset));
			}

			AttackPositions = points.ToArray();
		}
	}
}