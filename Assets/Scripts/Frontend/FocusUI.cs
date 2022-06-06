using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Frontend
{
	public sealed class FocusUI : MonoBehaviour
	{
		public delegate void FocusChangedDelegate(int patternIndex, int points);

		public event FocusChangedDelegate FocusChanged;

		[SerializeField] private TMP_InputField m_Points;
		[SerializeField] private TMP_Text m_PointsLeft;
		[SerializeField] private AttackPointUI[] m_AttackPoints;

		private int mPointsLeft;
		private int mPointsValue;

		private int PointsLeft
		{
			get => mPointsLeft;
			set
			{
				mPointsLeft = value;
				m_PointsLeft.SetText(mPointsLeft.ToString());
			}
		}
		private int PointsValue
		{
			get => mPointsValue;
			set
			{
				mPointsValue = Mathf.Clamp(value, 0, PointsLeft);
				m_Points.SetTextWithoutNotify(mPointsValue.ToString());
			}
		}

		public void OnAttackPointsChanged(IDictionary<int, int> attackPointsByIndex)
		{
			for (var i = 0; i < m_AttackPoints.Length; i++)
			{
				var patternIndex = ToPatternIndex(i);
				var isUsed = attackPointsByIndex.TryGetValue(patternIndex, out var points);
				m_AttackPoints[i].UpdateVisuals(points, isUsed);
			}
		}

		public void OnPointsLeftChanged(int pointsLeft)
		{
			PointsLeft = pointsLeft;
			PointsValue = PointsValue;
		}

		private void Awake()
		{
			for (var i = 0; i < m_AttackPoints.Length; i++)
			{
				var index = ToPatternIndex(i);
				m_AttackPoints[i].PointClicked += () => ChangeFocus(index);
			}
			m_Points.onValueChanged.AddListener(OnPointsValueChanged);

			foreach (var p in m_AttackPoints) p.UpdateVisuals(0, false);
		}

		private void OnPointsValueChanged(string input)
		{
			if (int.TryParse(m_Points.text, out var value)) PointsValue = value;
		}

		private void ChangeFocus(int index)
		{
			FocusChanged?.Invoke(index, PointsValue);
		}

		private static int ToPatternIndex(int i)
		{
			return i < 4 ? i : i + 1;
		}
	}
}