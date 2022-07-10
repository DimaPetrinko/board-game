using System;
using CoreMechanics.Actions;
using CoreMechanics.Units;
using CoreMechanics.Utilities;
using Frontend.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frontend.Testing.Game
{
	public class ActionRow : MonoBehaviour
	{
		[SerializeField] private TMP_Text m_UnitType;
		[SerializeField] private TMP_Text m_ActionPoints;
		[SerializeField] private Button m_Perform;
		[SerializeField] private TMP_Dropdown m_ActionType;
		[SerializeField] private Vec2IntInputField m_Position;
		[SerializeField] private GameObject m_PositionParent;
		[SerializeField] private TMP_Dropdown m_Orientation;
		[SerializeField] private GameObject m_OrientationParent;
		// TODO: focus

		private Unit mUnit;
		private ActionType[] mActionTypes;
		private Orientation[] mOrientations;

		private ActionType mCurrentAction;
		private Orientation mCurrentOrientation;
		private Vec2Int mCurrentPosition;

		private ActionType[] ActionTypes
		{
			get
			{
				if (mActionTypes == null) UIUtilities.SetUpDropdown(m_ActionType, out mActionTypes);
				return mActionTypes;
			}
		}

		private Orientation[] Orientations
		{
			get
			{
				if (mOrientations == null) UIUtilities.SetUpDropdown(m_Orientation, out mOrientations);
				return mOrientations;
			}
		}

		private void Awake()
		{
			m_Perform.onClick.AddListener(OnPerformClicked);
			m_ActionType.onValueChanged.AddListener(OnActionTypeChanged);
			m_Orientation.onValueChanged.AddListener(OnOrientationChanged);
			m_Position.onValueChanged.AddListener(OnPositionChanged);
			// TODO: focus

			OnActionTypeChanged(m_ActionType.value);
			OnOrientationChanged(m_Orientation.value);
			OnPositionChanged(m_Position.value);
		}

		public void AssignUnit(Unit unit)
		{
			mUnit = unit;

			m_UnitType.text = mUnit.Type.ToString();
			m_Orientation.value = Array.IndexOf(Orientations, mUnit.Orientation);
			m_Position.value = mUnit.Position;
			// TODO: focus
			OnActionPointsChanged();
		}

		public void ClearUnit()
		{
			mUnit = null;
		}

		public void OnActionPointsChanged()
		{
			m_ActionPoints.text = $"{mUnit.ActionPoints}/{mUnit.MaxActionPoints}";
			m_Perform.interactable = mUnit.ActionPoints > 0;
		}

		private void OnPerformClicked()
		{
			mUnit.RequestAction(mCurrentAction, GetExtraParameters());

			OnActionPointsChanged();
		}

		private void OnActionTypeChanged(int index)
		{
			mCurrentAction = ActionTypes[index];

			switch (mCurrentAction)
			{
				case ActionType.Move:
					m_PositionParent.SetActive(true);
					m_OrientationParent.SetActive(false);
					break;
				case ActionType.Rotate:
					m_PositionParent.SetActive(false);
					m_OrientationParent.SetActive(true);
					break;
				default:
					m_PositionParent.SetActive(false);
					m_OrientationParent.SetActive(false);
					break;
			}
		}

		private void OnOrientationChanged(int index)
		{
			mCurrentOrientation = Orientations[index];
		}

		private void OnPositionChanged(Vec2Int value)
		{
			mCurrentPosition = value;
		}

		private object GetExtraParameters()
		{
			return mCurrentAction switch
			{
				ActionType.AttackFocus => new FocusParameters(DefaultIndexPoints), // TODO: focus
				ActionType.Move => new MoveParameters(mCurrentPosition),
				ActionType.Rotate => new RotateParameters(mCurrentOrientation),
				_ => null
			};
		}

		private readonly IndexPoints[] DefaultIndexPoints =
		{
			new(0, 1), new(1, 1), new(2, 1),
			new(3, 1),                        new(5, 1),
			new(6, 1), new(7, 1), new(8, 1),
		};
	}
}