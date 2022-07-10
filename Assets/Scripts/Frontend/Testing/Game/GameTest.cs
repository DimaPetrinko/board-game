using System;
using CoreMechanics.Managers;
using CoreMechanics.Teams;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Frontend.Testing.Game
{
	public class GameTest : MonoBehaviour
	{
		private readonly struct Step
		{
			public readonly Action Action;
			public readonly string Name;

			public Step(Action action, string name)
			{
				Action = action;
				Name = name;
			}
		}

		[Header("Configs")]
		[SerializeField] private ConfigsManager m_ConfigsManager;

		[Header("Inputs/Outputs")]
		[SerializeField] private TMP_InputField m_Team1Name;
		[SerializeField] private TMP_InputField m_Team2Name;
		[SerializeField] private TMP_Text m_WinnerMessage;
		[SerializeField] private TMP_Text[] m_Team1Names;
		[SerializeField] private TMP_Text[] m_Team2Names;
		[SerializeField] private TeamUnitsConfigurator[] m_TeamUnitsConfigurators;
		[SerializeField] private TestTeamUI[] m_TeamUis;

		[Header("Steps")]
		[SerializeField] private GameObject m_TeamsCreation;
		[SerializeField] private GameObject m_UnitsConfiguration;
		[SerializeField] private GameObject m_GameUI;
		[SerializeField] private Button m_NextStep;
		[SerializeField] private TMP_Text m_CurrentStep;

		private GameManager mGameManager;
		private Step[] mSteps;
		private int mCurrentStep = 0;

		private void Awake()
		{
			mGameManager = new GameManager(m_ConfigsManager);

			mSteps = new Step[]
			{
				new(ResetUI, "Initialize"),
				new(OnCreateTeamsClicked, "Create teams"),
				new(OnAddUnitsForTeamsClicked, "Confirm units"),
				new(OnStartGameClicked, "Start game"),
				new(OnResetGameClicked, "Reset game")
			};

			m_NextStep.onClick.AddListener(OnNextStep);
		}

		private void Start()
		{
			OnNextStep();
		}

		private void OnNextStep()
		{
			mSteps[mCurrentStep].Action();
			mCurrentStep = (mCurrentStep + 1) % mSteps.Length;
			m_CurrentStep.text = mSteps[mCurrentStep].Name;
		}

		private void OnCreateTeamsClicked()
		{
			if (m_Team1Name.text.Equals(string.Empty)) m_Team1Name.text = "Team 1";
			if (m_Team2Name.text.Equals(string.Empty)) m_Team2Name.text = "Team 2";

			foreach (var teamName in m_Team1Names) teamName.text = m_Team1Name.text;
			foreach (var teamName in m_Team2Names) teamName.text = m_Team2Name.text;

			mGameManager.RegisterTeam(m_Team1Name.text);
			mGameManager.RegisterTeam(m_Team2Name.text);

			ToggleTeamCreationUI(false);
			ToggleUnitsConfigurators(true);
		}

		private void OnAddUnitsForTeamsClicked()
		{
			AddUnitsForTeam(0);
			AddUnitsForTeam(1);

			ToggleUnitsConfigurators(false);
		}

		private void OnStartGameClicked()
		{
			for (var i = 0; i < mGameManager.Teams.Length; i++) m_TeamUis[i].AssignTeam(mGameManager.Teams[i]);

			ToggleTeamUIs(true);

			mGameManager.StartGame();
			mGameManager.GameCompleted += OnGameCompleted;
		}

		private void OnResetGameClicked()
		{
			foreach (var teamUI in m_TeamUis) teamUI.ClearTeam();

			mGameManager.ResetGame();
			ResetUI();
		}

		private void ResetUI()
		{
			ToggleTeamCreationUI(true);
			ToggleUnitsConfigurators(false);
			ToggleTeamUIs(false);
			ToggleWinnerMessage(false);

			m_Team1Name.text = "Team 1";
			m_Team2Name.text = "Team 2";

			foreach (var teamName in m_Team1Names) teamName.text = m_Team1Name.text;
			foreach (var teamName in m_Team2Names) teamName.text = m_Team2Name.text;
		}

		private void OnGameCompleted(ITeam winner)
		{
			m_WinnerMessage.text = winner?.Name ?? "Draw";

			ToggleTeamUIs(false);
			ToggleWinnerMessage(true);
		}

		private void AddUnitsForTeam(int teamIndex)
		{
			var team = mGameManager.Teams[teamIndex];
			mGameManager.AddUnitsToTeam(team, m_TeamUnitsConfigurators[teamIndex].GetUnitsData());
		}

		private void ToggleTeamCreationUI(bool value)
		{
			m_TeamsCreation.SetActive(value);
		}

		private void ToggleUnitsConfigurators(bool value)
		{
			m_UnitsConfiguration.SetActive(value);
		}

		private void ToggleTeamUIs(bool value)
		{
			m_GameUI.SetActive(value);
		}

		private void ToggleWinnerMessage(bool value)
		{
			m_WinnerMessage.gameObject.SetActive(value);
		}
	}
}