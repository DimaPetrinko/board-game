using System.Collections.Generic;
using System.Linq;
using CoreMechanics.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Frontend.Testing.Game
{
	public class TeamUnitsConfigurator : MonoBehaviour
	{
		[SerializeField] private RectTransform m_UnitDataRowsParent;
		[SerializeField] private UnitDataRow m_UnitDataRowPrefab;

		[SerializeField] private Button m_AddUnit;
		[SerializeField] private Button m_RemoveLastUnit;

		private readonly Stack<UnitDataRow> mUnitRows = new();

		private void Awake()
		{
			m_AddUnit.onClick.AddListener(OnAddUnitClicked);
			m_RemoveLastUnit.onClick.AddListener(OnRemoveLastUnitClicked);
			m_UnitDataRowPrefab.gameObject.SetActive(false);
		}

		public IEnumerable<UnitTypePositionPair> GetUnitsData()
		{
			return mUnitRows.Select(u => u.GetData());
		}

		private void OnAddUnitClicked()
		{
			var row = Instantiate(m_UnitDataRowPrefab, m_UnitDataRowsParent);
			row.gameObject.SetActive(true);
			mUnitRows.Push(row);
		}

		private void OnRemoveLastUnitClicked()
		{
			var row = mUnitRows.Pop();
			Destroy(row.gameObject);
		}
	}
}