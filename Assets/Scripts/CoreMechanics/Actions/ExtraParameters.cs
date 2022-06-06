﻿using CoreMechanics.Units;
using CoreMechanics.Utilities;

namespace CoreMechanics.Actions
{
	public readonly struct RotateParameters
	{
		public readonly Orientation Orientation;

		public RotateParameters(Orientation orientation) => Orientation = orientation;
	}

	public readonly struct MoveParameters
	{
		public readonly Vec2Int Position;

		public MoveParameters(Vec2Int position) => Position = position;
	}

	public readonly struct FocusParameters
	{
		public readonly int PositionIndex;
		public readonly int Points;

		public FocusParameters(int positionIndex, int points)
		{
			PositionIndex = positionIndex;
			Points = points;
		}
	}
}