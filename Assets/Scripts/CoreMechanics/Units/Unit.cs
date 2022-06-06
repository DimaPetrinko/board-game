using System;
using System.Linq;
using CoreMechanics.Managers;
using CoreMechanics.Utilities;

namespace CoreMechanics.Units
{
	public class Unit
	{
		public event Action Died;

		private readonly IUnitConfig mConfig;
		private int mHealth;
		private int mActionPoints;
		private Vec2Int mPosition;
		private Orientation mOrientation;

		public int Health
		{
			get => mHealth;
			set
			{
				mHealth = Math.Clamp(value, 0, mConfig.Health);
				if (mHealth == 0) Died?.Invoke();
			}
		}
		public bool Dead => Health == 0;
		public int ActionPoints
		{
			get => mActionPoints;
			set => mActionPoints = Math.Clamp(value, 0, mConfig.ActionPoints);
		}
		public UnitType Type => mConfig.Type;
		public Vec2Int Position
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
		public AttackPosition[] AttackPositions { get; private set; }
		public int[] AttackPoints { get; }
		public int FreeAttackPoints => mConfig.AttackPoints - AttackPoints.Sum();

		public Unit(IUnitConfig config)
		{
			mConfig = config;

			Health = mConfig.Health;
			ActionPoints = mConfig.ActionPoints;
			AttackPoints = new int[mConfig.AttackPositions.Length];

			UpdateAttackPositions();
		}

		public int GetDamageForType(UnitType type)
		{
			mConfig.DamageByType.TryGetValue(type, out var damage);
			return damage;
		}

		public void AssignAttackPoint(int positionIndex, int points)
		{
			points = Math.Clamp(points, 0, FreeAttackPoints);
			AttackPoints[positionIndex] = points;
		}

		public void ResetActionPoints()
		{
			ActionPoints = mConfig.ActionPoints;
		}

		private void UpdateAttackPositions()
		{
			AttackPositions = AttackHandler.CreateAttackPositions(Position, Orientation, mConfig.AttackPositions);
		}
	}
}