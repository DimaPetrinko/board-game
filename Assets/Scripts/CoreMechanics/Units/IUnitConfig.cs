using System.Collections.ObjectModel;

namespace CoreMechanics.Units
{
	public interface IUnitConfig
	{
		int Health { get; }
		int ActionPoints { get; }
		UnitType Type { get; }
		ReadOnlyDictionary<UnitType, int> DamageByType { get; }
		int[] AttackPattern { get; }
	}
}