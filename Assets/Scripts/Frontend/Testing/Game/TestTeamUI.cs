using System.Collections.Generic;
using CoreMechanics.Teams;
using CoreMechanics.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Frontend.Testing.Game
{
	public class TestTeamUI : MonoBehaviour
	{
		[SerializeField] private CanvasGroup m_CanvasGroup;
		[SerializeField] private RectTransform m_ActionRowsParent;
		[SerializeField] private ActionRow m_ActionRowPrefab;
		[SerializeField] private Button m_EndTurn;

		private readonly Dictionary<Unit, ActionRow> mActionRows = new();

		private ITeam mTeam;

		public void AssignTeam(ITeam team)
		{
			mTeam = team;
			mTeam.TurnStarted += OnTurnStarted;
			mTeam.TurnCompleted += OnTurnCompleted;
			m_EndTurn.onClick.AddListener(mTeam.EndTurn);

			m_ActionRowPrefab.gameObject.SetActive(false);
			foreach (var unit in mTeam.Units) mActionRows.Add(unit, CreateActionRow(unit));
		}

		public void ClearTeam()
		{
			foreach (var (unit, row) in mActionRows)
			{
				unit.Died -= OnUnitDied;
				DestroyActionRow(row);
			}
			mActionRows.Clear();

			mTeam.TurnStarted -= OnTurnStarted;
			mTeam.TurnCompleted -= OnTurnCompleted;
			m_EndTurn.onClick.RemoveAllListeners();
			mTeam = null;
		}

		private void OnTurnStarted()
		{
			m_CanvasGroup.interactable = true;
			m_CanvasGroup.alpha = 1f;

			foreach (var row in mActionRows.Values) row.OnActionPointsChanged();
		}

		private void OnTurnCompleted()
		{
			m_CanvasGroup.interactable = false;
			m_CanvasGroup.alpha = 0.5f;
		}

		private void OnUnitDied(Unit unit)
		{
			unit.Died -= OnUnitDied;
			DestroyActionRow(mActionRows[unit]);
			mActionRows.Remove(unit);
		}

		private ActionRow CreateActionRow(Unit unit)
		{
			var row = Instantiate(m_ActionRowPrefab, m_ActionRowsParent);
			row.gameObject.SetActive(true);
			row.AssignUnit(unit);
			unit.Died += OnUnitDied;
			return row;
		}

		private void DestroyActionRow(ActionRow row)
		{
			row.ClearUnit();
			Destroy(row.gameObject);
		}
	}
}