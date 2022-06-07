using CoreMechanics.Board;
using CoreMechanics.Utilities;
using UnityEngine;

namespace Frontend.Configs
{
	[CreateAssetMenu(menuName = "Configs/Board", fileName = "Board", order = 0)]
	public class BoardConfig : ScriptableObject, IBoardConfig
	{
		[SerializeField] private Vec2Int m_BoardSize;

		public Vec2Int BoardSize => m_BoardSize;
	}
}