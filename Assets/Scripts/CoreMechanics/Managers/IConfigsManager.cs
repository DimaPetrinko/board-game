using System.Collections.Generic;
using CoreMechanics.Actions;
using CoreMechanics.Boards;
using CoreMechanics.Units;

namespace CoreMechanics.Managers
{
	public interface IConfigsManager
	{
		IBoardConfig BoardConfig { get;}
		IEnumerable<IActionConfig> ActionConfigs { get; }

		IUnitConfig GetUnitConfig(UnitType type);
	}
}