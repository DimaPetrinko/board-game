using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frontend
{
	[RequireComponent(typeof(Button))]
	public sealed class AttackPointUI : MonoBehaviour
	{
		public event Action PointClicked;

		[SerializeField] private TMP_Text m_Points;
		[SerializeField] private GameObject m_Active;

		private Button mButton;

		public void UpdateVisuals(int points, bool isUsed)
		{
			m_Active.SetActive(isUsed);
			if (points > 0)
			{
				m_Points.gameObject.SetActive(true);
				m_Points.text = points.ToString();
			}
			else m_Points.gameObject.SetActive(false);
		}

		private void Awake()
		{
			mButton = GetComponent<Button>();
			mButton.onClick.AddListener(() => PointClicked?.Invoke());
		}
	}
}