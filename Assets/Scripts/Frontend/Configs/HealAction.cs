using CoreMechanics.Actions;
using UnityEngine;

namespace Frontend.Configs
{
	[CreateAssetMenu(menuName = "Configs/HealAction", fileName = "Heal", order = 0)]
	public sealed class HealAction : ScriptableObject, IHealConfig
	{
		[SerializeField] private int m_Cost;
		[SerializeField] private int m_HealAmount;

		public ActionType Type => ActionType.Heal;
		public int Cost => m_Cost;
		public int HealAmount => m_HealAmount;
	}
}