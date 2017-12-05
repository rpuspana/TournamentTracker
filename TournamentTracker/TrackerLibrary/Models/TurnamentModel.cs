using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TurnamentModel
    {
        /// <summary>
        /// The name given to this tournament.
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// The amount of money each team needs to put up to enter.
        /// </summary>
        public decimal EntryFee { get; set; }

        /// <summary>
        /// The set of teams that have been entered.
        /// </summary>
        // inside the class constructor EnteredTeams = new List<TeamModel>();
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();

        /// <summary>
        /// The list of prizes for the various places.
        /// </summary>
        // inside the class constructor Prizes = new List<PrizeModel>();
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();

        /// <summary>
        /// The matchup per round.
        /// </summary>
        // inside the class constructor Rounds = new List<List<MatchupModel>>();
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();
    }
}
