using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        // contract for a method
        // everything in here is public because of the access modifier of the class
        /// <summary>
        /// Save a Prize model to the database or to a text file
        /// </summary>
        /// <param name="model">The prize information.</param>
        PrizeModel CreatePrize(PrizeModel model);

        /// <summary>
        /// Save a Person model to the database or to a text file
        /// </summary>
        /// <param name="model">The person information.</param>
        PersonModel CreatePerson(PersonModel model);

        /// <summary>
        /// Get all the people from the database or from a text file
        /// </summary>
        /// <param name="model">The person information.</param>
        /// <returns>A list of PesonModel instances from the database or fro a text file</returns>
        List<PersonModel> GetPersonAll();

        /// <summary>
        /// Insert every team member from a team in the TeamMembers table
        /// </summary>
        /// <param name="model">The team information</param>
        /// <returns>A TeamModel</returns>
        TeamModel CreateTeam(TeamModel model);

        /// <summary>
        /// Insert a tournament in the database/text file
        /// Insert all of the prizes ids in the database/text file
        /// Insert all of the teams ids in the database/text file
        /// </summary>
        /// <returns>A TournamentModel</returns>
        void CreateTournament(TournamentModel model);

        /// <summary>
        /// Get all the teams from the database or from a text file
        /// </summary>
        /// <returns>List of teams</returns>
        List<TeamModel> GetTeamAll();
    }
}
