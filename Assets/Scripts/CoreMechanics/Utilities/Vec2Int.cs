using System;

namespace CoreMechanics.Utilities
{
	[Serializable]
	public struct Vec2Int
	{
		public int x;
		public int y;

		public Vec2Int(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public bool Equals(Vec2Int other)
		{
			return x == other.x && y == other.y;
		}

		public override bool Equals(object obj)
		{
			return obj is Vec2Int other && Equals(other);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(x, y);
		}

		public static bool operator ==(Vec2Int left, Vec2Int right)
		{
			return left.x == right.x && left.y == right.y;
		}

		public static bool operator !=(Vec2Int left, Vec2Int right)
		{
			return !(left == right);
		}

		public static Vec2Int operator +(Vec2Int left, Vec2Int right)
		{
			left.x += right.x;
			left.y += right.y;
			return left;
		}

		public static Vec2Int operator -(Vec2Int left, Vec2Int right)
		{
			left.x -= right.x;
			left.y -= right.y;
			return left;
		}

		public static Vec2Int operator *(Vec2Int vector, int scalar)
		{
			vector.x *= scalar;
			vector.y *= scalar;
			return vector;
		}
	}
}