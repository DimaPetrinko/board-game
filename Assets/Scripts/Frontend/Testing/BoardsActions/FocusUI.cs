using System;
using System.Collections.Generic;
using System.Linq;
using CoreMechanics.Actions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frontend.Testing.BoardsActions
{
	public sealed class FocusUI : MonoBehaviour
	{
		public event Action<FocusParameters> FocusApplied;

		[SerializeField] private TMP_InputField m_Points;
		[SerializeField] private TMP_Text m_PointsLeft;
		[SerializeField] private AttackPointUI[] m_AttackPoints;
		[SerializeField] private Button m_Apply;
		[SerializeField] private GameObject m_DirtyIndicator;

		private IDictionary<int, int> mAttackPointsByIndex;

		private int mOriginalPointsLeft;
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
			mAttackPointsByIndex = attackPointsByIndex;
			for (var i = 0; i < m_AttackPoints.Length; i++)
			{
				var patternIndex = ToPatternIndex(i);
				var isUsed = attackPointsByIndex.TryGetValue(patternIndex, out var points);
				m_AttackPoints[i].UpdateVisuals(points, isUsed);
			}
		}

		public void OnPointsLeftChanged(int pointsLeft)
		{
			if (mOriginalPointsLeft == 0) mOriginalPointsLeft = pointsLeft;
			PointsLeft = pointsLeft;
			PointsValue = PointsValue;
		}

		public void OnApplied()
		{
			m_DirtyIndicator.SetActive(false);
			m_Apply.interactable = false;
		}

		private void Awake()
		{
			for (var i = 0; i < m_AttackPoints.Length; i++)
			{
				var index = ToPatternIndex(i);
				m_AttackPoints[i].PointClicked += () => ChangeFocus(index);
			}
			m_Points.onValueChanged.AddListener(OnPointsValueChanged);
			m_Apply.onClick.AddListener(Apply);

			foreach (var p in m_AttackPoints) p.UpdateVisuals(0, false);
		}

		private void OnPointsValueChanged(string input)
		{
			if (int.TryParse(m_Points.text, out var value)) PointsValue = value;
		}

		private void ChangeFocus(int index)
		{
			if (!mAttackPointsByIndex.ContainsKey(index)
				|| PointsValue == 0 && mAttackPointsByIndex[index] == 0)
				return;
			mAttackPointsByIndex[index] = PointsValue;
			OnPointsLeftChanged(mOriginalPointsLeft - mAttackPointsByIndex.Sum(p => p.Value));
			OnAttackPointsChanged(mAttackPointsByIndex);

			m_DirtyIndicator.SetActive(true);
			m_Apply.interactable = true;
		}

		private void Apply()
		{
			var points = mAttackPointsByIndex
				.Select((p, i) => new IndexPoints(i, p.Value))
				.ToArray();
			FocusApplied?.Invoke(new FocusParameters(points));
		}

		private static int ToPatternIndex(int i)
		{
			return i < 4 ? i : i + 1;
		}
	}
}