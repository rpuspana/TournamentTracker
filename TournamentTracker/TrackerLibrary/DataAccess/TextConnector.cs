using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string _PrizesFile       = "PrizeModels.csv";
        private const string _PeopleFile       = "PersonModels.csv";
        private const string _TeamFile         = "TeamModels.csv";
        private const string _TournamentsFile  = "TournamentModels.csv";
        private const string _MatchupFile      = "MatchupModels.csv";
        private const string _MatchupEntryFile = "MatchupEntryModels";

        public PersonModel CreatePerson(PersonModel model)
        {
            // As a result of the right hand expresion we will have an empty list if the PersonModels.csv file doesn't exist
            // or a list of PersonModel objects, each one having the values of a text file's row
            List<PersonModel> people = _PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();

            int currentId = 1;

            if (people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }

            // add the id to the PersonModel created with user input values from a form
            model.Id = currentId;

            // add the new person created by the user to the list of people that will be written to the file
            people.Add(model);

            people.SaveToPeopleFile(_PeopleFile);

            return model;
        }

        public PrizeModel CreatePrize(PrizeModel model)
        {
            // Load the text file and convert teh text to List<PrizeModel>
            // Statement explanation : pass the string from _PrizesFile and pass it to FullFilePath()
            // pass the result to LoadFile()
            // pass the result to ConvertToPrizeModels()
            // As a result of the right hand expresion we will have an empty list if the PrizeModels.csv file doesn't exist
            // or a list of PrizeModel objects, each one having the values of a text file's row
            List<PrizeModel> prizes = _PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            int newId = 1;

            if (prizes.Count > 0)
            {
                // Read through to find the highest id and add 1
                // Statement explanation : order the PrizeModel list by the PrizeModel Id instance var in descending order, 
                //                         get the first PrizeModel object, get it's id
                newId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }

            // add the id to the PersonModel created with user input values from a form
            // Use the highest id + 1 as the new id
            model.Id = newId;

            // add the new prize created by the user to the list of prizes that will be written to the file
            prizes.Add(model);

            // Convert the prizes to a list<string>
            // Save the list<string> to the text file
            prizes.SaveToPrizeFile(_PrizesFile);

            return model;
        }

        /// <summary>
        /// Insert every team member from a team in the TeamMembers table
        /// </summary>
        /// <param name="model">The team information</param>
        /// <returns>A Team model</returns>
        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = _TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(_PeopleFile);

            int newId = 1;

            if (teams.Count > 0)
            {
                // Read through to find the highest id and add 1
                // Statement explanation : order the TeamModel list by the TeamModel Id instance var in descending order, 
                //                         get the first TeamModel object, get it's id
                newId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }

            // add the id to the TeamModel created with user input values from a form
            // Use the highest id + 1 as the new id
            model.Id = newId;

            // add the new team created by the user to the list of teams that will be written to the file
            teams.Add(model);

            teams.SaveToTeamFile(_TeamFile);

            return model;
        }

        /// <summary>
        /// Insert a tournament in to a database/text file
        /// </summary>
        /// <returns>A TournamentModel</returns>
        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = _TeamFile
                                                .FullFilePath()
                                                .LoadFile()
                                                .ConvertToTournamentModels(_TeamFile, _PeopleFile, _PrizesFile);

            int currentId = 1;

            if (tournaments.Count > 0)
            {
                // Read through to find the highest id and add 1
                // Statement explanation : order the TournamentsModel list by the TournamentsModel Id instance var in descending order, 
                //                         get the first TournamentsModel object, get it's id
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;

                model.Id = currentId;

                model.SaveRoundsToFile(_MatchupFile, _MatchupEntryFile);

                tournaments.Add(model);

                tournaments.SaveToTournamentFile(_TournamentsFile);
            }
        }

        /// <summary>
        /// Gets all the people from PersonModel.csv
        /// </summary>
        /// <returns>A list of PesonModel instances from PersonModel.csv</returns>
        public List<PersonModel> GetPersonAll()
        {
            return _PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        /// <summary>
        /// Gets all the teams from TeamModel.csv
        /// </summary>
        /// <returns>A list of PesonModel instances from PersonModel.csv</returns>
        public List<TeamModel> GetTeamAll()
        {
            return _TeamFile.FullFilePath().LoadFile().ConvertToTeamModels(_PeopleFile);
        }
    }
}
