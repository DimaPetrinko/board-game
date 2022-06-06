using CoreMechanics.Actions;
using UnityEngine;

namespace Frontend.Configs
{
	[CreateAssetMenu(menuName = "Configs/HealAction", fileName = "Heal", order = 0)]
	public sealed class HealAction : ActionConfig, IHealConfig
	{
		[SerializeField] private int m_HealAmount;

		public new ActionType Type => ActionType.Heal;
		public int HealAmount => m_HealAmount;
	}
}