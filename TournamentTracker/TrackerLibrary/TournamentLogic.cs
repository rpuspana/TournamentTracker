using System;
using System.Collections.Generic;
using System.Linq.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        /// <summary>
        /// Create the rounds
        /// </summary>
        /// <param name="model"></param>
        public static void CreateRounds(TournamentModel model)
        {
            // Order our list of teams randomly
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);

            // number of rounds based of the number of teams in the tournament
            int rounds = FindNumberOfRounds(randomizedTeams.Count);

            // byeTeam = dummy team that competes against one team.
            // Used when the number of teams is odd
            int byes = NumberOfByeTeams(rounds, randomizedTeams.Count);
            
            // Create our first round of matchups
            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));

            // Create every round after that - 8 teams 4 matchups => 4 teams 2 matchups = 2 teams 1 matchup => winner
            CreateOtherRounds(model, rounds);
        }

        /// <summary>
        /// Create all the rounds, except the first one
        /// </summary>
        /// <param name="model">Tournament model</param>
        /// <param name="rounds">Number of rounds of the tournament</param>
        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            // the current round we are on
            int round = 2;

            // grab the first list of Matchup model
            // initially this is the first round
            List<MatchupModel> previousRound = model.Rounds[0];

            List<MatchupModel> currentRound = new List<MatchupModel>();

            MatchupModel currentMatchup = new MatchupModel();

            while(round <= rounds)
            {
                // loop through the previous round
                // the first person from the competing teams gets the bye-team
                // Eg from Matchup System logic - Sue VS Bye
                foreach (MatchupModel match in previousRound)
                {
                    // I don't know the score or the team
                    currentMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match });

                    if (currentMatchup.Entries.Count > 1)
                    {
                        currentMatchup.MatchupRound = round;
                        currentRound.Add(currentMatchup);
                        currentMatchup = new MatchupModel();
                    }
                }

                model.Rounds.Add(currentRound);
                previousRound = currentRound;

                currentRound = new List<MatchupModel>();
                round += 1;
            }
        }

        /// <summary>
        /// Create the first round of the tournament
        /// </summary>
        /// <param name="byes">Number of bye-teams</param>
        /// <param name="teams">Number of teams</param>
        /// <returns></returns>
        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel currentModel = new MatchupModel();

            foreach(TeamModel team in teams)
            {
                // add a team to perform on this match
                currentModel.Entries.Add(new MatchupEntryModel { TeamCompeting = team });

                // if I have a bye-team still available, I am done with this matchup
                // or no bye-teams but there's two entries in this list = two teams
                if (byes > 0 || currentModel.Entries.Count > 1)
                {
                    // the current match belongs to the first round
                    currentModel.MatchupRound = 1;
                    output.Add(currentModel);
                    currentModel = new MatchupModel();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }

            return output;
        }

        private static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int val = 2;

            // how many rounds I need to go
            while(val < teamCount)
            {
                output += 1;
                val = val * 2;
            }

            // number of rounds
            return output;
        }

        /// <summary>
        /// Randomize de order of teams in the paramter list of teams
        /// </summary>
        /// <param name="teams">List of teams to be randomized</param>
        /// <returns>aA new list with the teams randomized</returns>
        public static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            // take a list in
            // return an ordered list ordered by the GUID assigned to every row
            //return teams.OrderBy<TeamModel, int>(team => Guid.NewGuid).ToList();

            return teams.AsQueryable().OrderBy(a => Guid.NewGuid()).ToList();
        }

        /// <summary>
        /// Calculate how many dummy teams, or byes, will be in the tournament
        /// </summary>
        /// <param name="rounds">Number of rounds the tournament will have</param>
        /// <param name="numberOfTeams">Number of teams competing in the tournament</param>
        /// <returns>Number of dummy teams, or byes, will be in the tournament</returns>
        private static int NumberOfByeTeams(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams = totalTeams * 2;
            }

            output = totalTeams - numberOfTeams;

            return output;
        }
    }
}
