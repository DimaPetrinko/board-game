namespace CoreMechanics.Actions
{
	public interface IActionConfig
	{
		ActionType Type { get; }
		int Cost { get; }
	}

	public interface IHealConfig : IActionConfig
	{
		int HealAmount { get; }
	}
}