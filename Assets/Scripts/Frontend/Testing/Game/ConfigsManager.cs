using System.Collections.Generic;
using System.Linq;
using CoreMechanics.Actions;
using CoreMechanics.Boards;
using CoreMechanics.Managers;
using CoreMechanics.Units;
using Frontend.Configs;
using UnityEngine;

namespace Frontend.Testing.Game
{
	[CreateAssetMenu(menuName = "Configs/ConfigsManager", fileName = "ConfigsManager", order = 0)]
	public class ConfigsManager : ScriptableObject, IConfigsManager
	{
		[SerializeField] private BoardConfig m_BoardConfig;
		[SerializeField] private ActionConfig[] m_ActionConfigs;
		[SerializeField] private UnitConfig[] m_UnitConfigs;

		private Dictionary<UnitType, IUnitConfig> mUnitConfigsByType;

		private Dictionary<UnitType, IUnitConfig> UnitConfigsByType
		{
			get
			{
				if (mUnitConfigsByType == null
					|| mUnitConfigsByType.Count != m_UnitConfigs.Length)
				{
					mUnitConfigsByType = m_UnitConfigs
						.ToDictionary(u => u.Type, u => (IUnitConfig)u);
				}

				return mUnitConfigsByType;
			}
		}

		public IBoardConfig BoardConfig => m_BoardConfig;
		public IEnumerable<IActionConfig> ActionConfigs => m_ActionConfigs;

		public IUnitConfig GetUnitConfig(UnitType type)
		{
			return UnitConfigsByType[type];
		}
	}
}