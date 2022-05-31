using System;
using UnityEngine;

namespace CoreMechanics
{
	public class Unit
	{
		public event Action Died;

		private readonly UnitConfig mConfig;
		private int mHealth;
		private Vector2Int mPosition;
		private Orientation mOrientation;

		public int Health
		{
			get => mHealth;
			set
			{
				mHealth = Mathf.Clamp(value, 0, mConfig.Health);
				if (mHealth == 0) Died?.Invoke();
			}
		}
		public bool Dead => Health == 0;
		public UnitType Type => mConfig.Type;
		public Vector2Int Position
		{
			get => mPosition;
			set
			{
				mPosition = value;
				UpdateAttackPositions();
			}
		}
		public Orientation Orientation
		{
			get => mOrientation;
			set
			{
				mOrientation = value;
				UpdateAttackPositions();
			}
		}
		public Vector2Int[] AttackPositions { get; private set; }

		public Unit(UnitConfig config)
		{
			mConfig = config;

			Health = mConfig.Health;
		}

		public int GetDamageForType(UnitType type)
		{
			mConfig.DamageByType.TryGetValue(type, out var damage);
			return damage;
		}

		private void UpdateAttackPositions()
		{
			AttackPositions = AttackHandler.CreateAttackPositions(Position, Orientation, mConfig);
		}
	}
}