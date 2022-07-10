using CoreMechanics.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Frontend.Testing.Game
{
	public class Vec2IntInputField : MonoBehaviour
	{
		public UnityEvent<Vec2Int> onValueChanged;

		[SerializeField] private TMP_InputField m_X;
		[SerializeField] private TMP_InputField m_Y;

		private Vec2Int mValue;

		public Vec2Int value
		{
			get => mValue;
			set
			{
				SetValueWithoutNotify(value);
				onValueChanged?.Invoke(value);
			}
		}

		public void SetValueWithoutNotify(Vec2Int v)
		{
			m_X.SetTextWithoutNotify(v.x.ToString());
			m_Y.SetTextWithoutNotify(v.y.ToString());
			mValue = v;
		}

		private void Awake()
		{
			m_X.onValueChanged.AddListener(OnValueChanged);
			m_Y.onValueChanged.AddListener(OnValueChanged);
		}

		private void OnValueChanged(string _)
		{
			int.TryParse(m_X.text, out mValue.x);
			int.TryParse(m_Y.text, out mValue.y);
			onValueChanged?.Invoke(value);
		}
	}
}