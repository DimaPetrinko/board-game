using System.Linq;
using CoreMechanics.Actions;
using CoreMechanics.Managers;
using CoreMechanics.Units;
using CoreMechanics.Utilities;
using Frontend.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frontend
{
	public sealed class TestUI : MonoBehaviour
	{
		[Header("Unit")]
		[SerializeField] private UnitConfig m_Config;
		[SerializeField] private ActionConfig[] m_ActionConfigs;
		[SerializeField] private UnitPresenter m_UnitPresenter;
		[SerializeField] private Button m_ResetActionPoints;
		[SerializeField] private TMP_Text m_ActionPoints;

		[Header("Movement")]
		[SerializeField] private Button m_Up;
		[SerializeField] private Button m_Right;
		[SerializeField] private Button m_Down;
		[SerializeField] private Button m_Left;
		[SerializeField] private Button m_RotateCw;
		[SerializeField] private Button m_RotateCww;

		[Header("Attack")]
		[SerializeField] private Button m_Attack;
		[SerializeField] private FocusUI m_FocusUI;
		[SerializeField] private Button m_Heal;
		[SerializeField] private TMP_Text m_Health;

		private Unit mUnit;
		private ActionManager mActionManager;

		private void Awake()
		{
			mUnit = new Unit(m_Config);
			mActionManager = new ActionManager(m_ActionConfigs, new FakeBoard());

			m_Up.onClick.AddListener(() => Move(mUnit.Position + new Vec2Int(0, 1)));
			m_Right.onClick.AddListener(() => Move(mUnit.Position + new Vec2Int(1, 0)));
			m_Down.onClick.AddListener(() => Move(mUnit.Position + new Vec2Int(0, -1)));
			m_Left.onClick.AddListener(() => Move(mUnit.Position + new Vec2Int(-1, 0)));
			m_RotateCw.onClick.AddListener(() => Rotate(mUnit.Orientation + 1));
			m_RotateCww.onClick.AddListener(() => Rotate(mUnit.Orientation - 1));
			m_ResetActionPoints.onClick.AddListener(() =>
			{
				mUnit.ResetActionPoints();
				UpdateUnitVisuals();
			});

			m_Attack.onClick.AddListener(Attack);
			m_Heal.onClick.AddListener(Heal);
			m_FocusUI.FocusChanged += OnFocusChanged;

			UpdateFocusUI();
			UpdateUnitVisuals();
		}

		private void Move(Vec2Int position)
		{
			mActionManager.PerformAction(ActionType.Move, mUnit, new MoveParameters(position));
			UpdateUnitVisuals();
		}

		private void Rotate(Orientation orientation)
		{
			mActionManager.PerformAction(ActionType.Rotate, mUnit, new RotateParameters(orientation));
			UpdateUnitVisuals();
		}

		private void Attack()
		{
			mActionManager.PerformAction(ActionType.Attack, mUnit);
			UpdateUnitVisuals();
		}

		private void Heal()
		{
			mActionManager.PerformAction(ActionType.Heal, mUnit);
			UpdateUnitVisuals();
		}

		private void OnFocusChanged(int patternIndex, int points)
		{
			var attackPointIndex = -1;
			for (var i = 0; i < mUnit.AttackPositions.Length; i++)
			{
				if (mUnit.AttackPositions[i].PatternIndex != patternIndex) continue;
				attackPointIndex = i;
				break;
			}

			if (attackPointIndex < 0) return;

			if (points == 0 && mUnit.AttackPoints[attackPointIndex] == 0) return;

			mActionManager.PerformAction(
				ActionType.AttackFocus,
				mUnit,
				new[] {new FocusParameters(attackPointIndex, points)});

			UpdateFocusUI();
			UpdateUnitVisuals();
		}

		private void UpdateFocusUI()
		{
			var attackPointsByIndex = mUnit.AttackPoints
				.Zip(mUnit.AttackPositions, (point, position) => new {Index = position.PatternIndex, Point = point})
				.ToDictionary(p => p.Index, p => p.Point);

			m_FocusUI.OnPointsLeftChanged(mUnit.FreeAttackPoints);
			m_FocusUI.OnAttackPointsChanged(attackPointsByIndex);
		}

		private void UpdateUnitVisuals()
		{
			m_UnitPresenter.Present(mUnit);
			m_ActionPoints.text = mUnit.ActionPoints.ToString();
			m_Health.text = mUnit.Health.ToString();
		}
	}
}