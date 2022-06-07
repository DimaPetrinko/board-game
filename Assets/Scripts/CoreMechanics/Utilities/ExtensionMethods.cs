using System;
using CoreMechanics.Units;

namespace CoreMechanics.Utilities
{
	public static class ExtensionMethods
	{
		public static Orientation Wrap(this Orientation orientation)
		{
			var count = Enum.GetValues(typeof(Orientation)).Length;
			return (Orientation)(((int)orientation + count) % count);
		}
	}
}