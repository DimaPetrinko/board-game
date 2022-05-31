using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace CoreMechanics
{
	[CreateAssetMenu(menuName = "Configs/Unit", fileName = "Unit", order = 0)]
	public sealed class UnitConfig : ScriptableObject
	{
		[Serializable]
		public struct TypeDamage
		{
			public UnitType Type;
			public int Damage;
		}

		[SerializeField] private int m_Health;
		[SerializeField] private int m_Movement;
		[SerializeField] private UnitType m_Type;
		[SerializeField] private TypeDamage[] m_DamageByType;
		[SerializeField] [TextArea] private string m_AttackPattern;

		public int Health => m_Health;
		public int Movement => m_Movement;
		public UnitType Type => m_Type;
		public ReadOnlyDictionary<UnitType, int> DamageByType { get; private set; }
		public int[] AttackPattern { get; private set; }

		private void OnValidate()
		{
			DamageByType = new ReadOnlyDictionary<UnitType, int>(m_DamageByType
				.ToDictionary(p => p.Type, p => p.Damage));
			AttackPattern = m_AttackPattern
				.Where(ch => !ch.Equals('\n'))
				.Select(ch => int.Parse(ch.ToString()))
				.ToArray();
		}
	}
}