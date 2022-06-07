using System.Linq;
using CoreMechanics.Actions;
using CoreMechanics.Board;
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
		[Header("Board")]
		[SerializeField] private BoardConfig m_BoardConfig;

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
		[SerializeField] private Button m_RotateNorth;
		[SerializeField] private Button m_RotateEast;
		[SerializeField] private Button m_RotateSouth;
		[SerializeField] private Button m_RotateWest;

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
			var secondUnit = new Unit(m_Config);
			var board = new Board(m_BoardConfig);
			board.AddUnit(mUnit, new Vec2Int(0, 0));
			board.AddUnit(secondUnit, new Vec2Int(0, 4));

			mActionManager = new ActionManager(m_ActionConfigs, board);

			mActionManager.PerformAction(ActionType.Rotate, secondUnit, new RotateParameters(Orientation.South));
			secondUnit.ResetActionPoints();
			mActionManager.PerformAction(ActionType.AttackFocus, secondUnit, new[] {new FocusParameters(1, 3)});
			secondUnit.ResetActionPoints();

			var secondPresenter = Instantiate(m_UnitPresenter);
			secondPresenter.Present(secondUnit);
			secondUnit.Died += _ => Destroy(secondPresenter.gameObject);

			m_Up.onClick.AddListener(() => Move(mUnit.Position + new Vec2Int(0, 1)));
			m_Right.onClick.AddListener(() => Move(mUnit.Position + new Vec2Int(1, 0)));
			m_Down.onClick.AddListener(() => Move(mUnit.Position + new Vec2Int(0, -1)));
			m_Left.onClick.AddListener(() => Move(mUnit.Position + new Vec2Int(-1, 0)));
			m_RotateNorth.onClick.AddListener(() => Rotate(Orientation.North));
			m_RotateEast.onClick.AddListener(() => Rotate(Orientation.East));
			m_RotateSouth.onClick.AddListener(() => Rotate(Orientation.South));
			m_RotateWest.onClick.AddListener(() => Rotate(Orientation.West));
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
			var attackPointIndex = int.MaxValue;
			for (var i = 0; i < mUnit.AttackPositions.Length; i++)
			{
				if (mUnit.AttackPositions[i].PatternIndex != patternIndex) continue;
				attackPointIndex = i;
				break;
			}

			mActionManager.PerformAction(
				ActionType.AttackFocus,
				mUnit,
				new[] {new FocusParameters(attackPointIndex, points)});

			UpdateFocusUI();
			UpdateUnitVisuals();
		}

		private void UpdateFocusUI()
		{
			var attackPointsByIndex = mUnit.AttackPositions
				.ToDictionary(p => p.PatternIndex, p => p.Points);

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