using System;
using System.Collections.Generic;
using System.Linq;
using CoreMechanics.Teams;
using CoreMechanics.Units;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frontend.Testing.TurnsTeams
{
	public class TestTeam : MonoBehaviour, ITeam
	{
		public event Action TurnStarted;
		public event Action TurnCompleted;
		public event UnitActionRequest UnitRequestedAction;

		[SerializeField] private Button m_EndTurnButton;
		[SerializeField] private Button m_IncreaseUnitsButton;
		[SerializeField] private Button m_DecreaseUnitsButton;
		[SerializeField] private TMP_Text m_Units;

		private int mUnitsLeft;
		private int mMaxUnits;

		public string Name => gameObject.name;
		public bool IsDefeated => mUnitsLeft == 0;
		public Unit[] Units { get; }

		public void AddUnits(IEnumerable<Unit> units)
		{
			mMaxUnits = units.Count();
			mUnitsLeft = mMaxUnits;
		}

		public void RemoveAllUnits()
		{
			mUnitsLeft = 0;
		}

		public void StartTurn()
		{
			m_EndTurnButton.interactable = true;
			TurnStarted?.Invoke();
		}

		public void EndTurn()
		{
			TurnCompleted?.Invoke();
			m_EndTurnButton.interactable = false;
		}

		private void Awake()
		{
			m_EndTurnButton.onClick.AddListener(OnEndTurnButtonClicked);
			m_IncreaseUnitsButton.onClick.AddListener(() => OnChangeUnitsAmountButtonClicked(1));
			m_DecreaseUnitsButton.onClick.AddListener(() => OnChangeUnitsAmountButtonClicked(-1));
			m_EndTurnButton.interactable = false;
		}

		private void OnEndTurnButtonClicked()
		{
			EndTurn();
		}

		private void OnChangeUnitsAmountButtonClicked(int delta)
		{
			mUnitsLeft = Mathf.Clamp(mUnitsLeft + delta, 0, mMaxUnits);
			m_Units.text = mUnitsLeft.ToString();
		}
	}
}