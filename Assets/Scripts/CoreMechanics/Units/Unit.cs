using System;
using System.Linq;
using CoreMechanics.Actions;
using CoreMechanics.Managers;
using CoreMechanics.Utilities;

namespace CoreMechanics.Units
{
	public readonly struct UnitTypePositionPair
	{
		public readonly UnitType UnitType;
		public readonly Vec2Int Position;

		public UnitTypePositionPair(UnitType unitType, Vec2Int position)
		{
			UnitType = unitType;
			Position = position;
		}
	}
	public delegate void UnitEvent(Unit sender);
	public delegate void UnitActionRequest(Unit sender, ActionType actionType, object extraParameters = null);

	public sealed class Unit
	{
		public event UnitEvent Died;
		public event UnitActionRequest ActionRequested;

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
				if (mHealth == 0) Died?.Invoke(this);
			}
		}
		public bool Healthy => Health == mConfig.Health;
		public bool Dead => Health == 0;
		public int ActionPoints
		{
			get => mActionPoints;
			set => mActionPoints = Math.Clamp(value, 0, mConfig.ActionPoints);
		}
		public bool ReturnAttack => mConfig.ReturnAttack;
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
		public int FreeAttackPoints => mConfig.AttackPoints - AttackPositions.Sum(p => p.Points);

		public Unit(IUnitConfig config)
		{
			mConfig = config;

			Health = mConfig.Health;
			ActionPoints = mConfig.ActionPoints;

			AttackPositions = mConfig.AttackPositions;
		}

		public int GetDamageForType(UnitType type)
		{
			mConfig.DamageByType.TryGetValue(type, out var damage);
			return damage;
		}

		public void AssignAttackPoint(int positionIndex, int points)
		{
			points = Math.Clamp(points, 0, FreeAttackPoints);
			AttackPositions[positionIndex].Points = points;
		}

		public void ResetActionPoints()
		{
			ActionPoints = mConfig.ActionPoints;
		}

		public void RequestAction(ActionType actionType, object extraParameters = null)
		{
			ActionRequested?.Invoke(this, actionType, extraParameters);
		}

		private void UpdateAttackPositions()
		{
			AttackPositions = AttackHandler.CreateAttackPositions(Position,
				Orientation, AttackPositions, mConfig.AttackPositions);
		}
	}
}