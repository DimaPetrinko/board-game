using System.Linq;
using CoreMechanics.Teams;

namespace CoreMechanics.Managers
{
	// turn manager manages turns, duh
	// only one team is doing the turn
	// during the turn the team uses action points on (one/some/all) units at their disposal
	// each action has action points (AP) cost
	// each unit has AP per turn

	public class TurnManager
	{
		public delegate void GameCompletedDelegate(ITeam winner);

		public event GameCompletedDelegate GameCompleted;

		private readonly ITeam[] mTeams;
		private int mCurrentTeam;

		private int CurrentTeam
		{
			get => mCurrentTeam;
			set => mCurrentTeam = (value + mTeams.Length) % mTeams.Length;
		}

		public TurnManager(ITeam[] teams)
		{
			mTeams = teams;
		}

		public void StartGame()
		{
			NextTurn();
			foreach (var team in mTeams) team.EndTurn();
		}

		private void NextTurn()
		{
			var team = mTeams[CurrentTeam];
			team.TurnCompleted += OnTurnCompleted;
			team.StartTurn();
		}

		private void OnTurnCompleted()
		{
			var team = mTeams[CurrentTeam];
			team.TurnCompleted -= OnTurnCompleted;

			if (mTeams.Count(t => !t.IsDefeated) > 1)
			{
				CurrentTeam++;
				NextTurn();
			}
			else
			{
				var winner = mTeams.FirstOrDefault(t => !t.IsDefeated);
				GameCompleted?.Invoke(winner);
			}
		}
	}
}