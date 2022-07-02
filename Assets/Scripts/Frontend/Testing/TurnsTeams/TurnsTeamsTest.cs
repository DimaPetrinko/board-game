using CoreMechanics.Managers;
using CoreMechanics.Teams;
using TMPro;
using UnityEngine;

namespace Frontend.Testing.TurnsTeams
{
	public class TurnsTeamsTest : MonoBehaviour
	{
		[SerializeField] private TestTeam[] m_Teams;
		[SerializeField] private TMP_Text m_GameEndedText;

		private void Awake()
		{
			var turnManager = new TurnManager(m_Teams);
			turnManager.GameCompleted += OnGameCompleted;
			m_GameEndedText.gameObject.SetActive(false);

			turnManager.StartGame();
		}

		private void OnGameCompleted(ITeam winner)
		{
			m_GameEndedText.gameObject.SetActive(true);
			m_GameEndedText.text = winner == null ? "Draw" : $"{winner.Name} won!";
		}
	}
}