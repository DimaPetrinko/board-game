using CoreMechanics.Units;
using Frontend.Utilities;
using TMPro;
using UnityEngine;

namespace Frontend.Testing.Game
{
	public class UnitDataRow : MonoBehaviour
	{
		[SerializeField] private TMP_Dropdown m_UnitType;
		[SerializeField] private Vec2IntInputField m_Position;

		private UnitType[] mUnitTypes;

		private void Awake()
		{
			UIUtilities.SetUpDropdown(m_UnitType, out mUnitTypes);
		}

		public UnitTypePositionPair GetData()
		{
			return new UnitTypePositionPair(mUnitTypes[m_UnitType.value], m_Position.value);
		}
	}
}