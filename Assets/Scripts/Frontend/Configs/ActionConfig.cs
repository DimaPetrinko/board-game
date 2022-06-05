using CoreMechanics.Actions;
using UnityEngine;

namespace Frontend.Configs
{
	[CreateAssetMenu(menuName = "Configs/Action", fileName = "Action", order = 0)]
	public class ActionConfig : ScriptableObject, IActionConfig
	{
		[SerializeField] private ActionType m_ActionType;
		[SerializeField] private int m_Cost;

		public ActionType Type => m_ActionType;
		public int Cost => m_Cost;
	}
}