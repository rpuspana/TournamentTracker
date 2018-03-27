using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TrackerLibrary.Models;

// * Load the text file
// * Convert teh text to List<PrizeModel>
// Read through to find the highest id
// Use the highest id + 1 as the new id
// Convert the prizes to a list<string>
// Save the list<string> to the text file

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        // extension method
        /// <summary>
        /// Return the full path of a file
        /// </summary>
        /// <param name="fileName">The file name</param>
        /// <returns>The full path of a file passed in</returns>
        public static string FullFilePath(this string fileName) // PrizeModels.csv
        {
            // return the value from App.config file -> appSettings tag -> filePath key
            // => C:\personal\learn\C#\TournamentTracker\fileStorag\PrizeModels.csv
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName }";
        }

        /// <summary>
        /// Take the full file path and load that file
        /// </summary>
        /// <param name="file">File to be loaded</param>
        /// <returns>List of prize strings</returns>
        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        /// <summary>
        /// Convert lines from a csv file to PrizeModel instances
        /// </summary>
        /// <param name="lines">Lines of text from a csv file</param>
        /// <returns>List of PrizeModel objects created from each line of a csv file</returns>
        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            // In order for the code to work, the content of the file needs to be in the correct order
            // and needs to have valid data
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);

                output.Add(p);
            }

            return output;
        }

        /// <summary>
        /// Convert lines from a csv file to PersonMldel instances
        /// </summary>
        /// <param name="lines">Lines of text from a csv file</param>
        /// <returns>List of PersonModel objects created from each line of a csv file</returns>
        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            // In order for the code to work, the content of the file needs to be in the correct order
            // and needs to have valid data
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PersonModel p = new PersonModel();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1].Trim();
                p.LastName = cols[2].Trim();
                p.EmailAddress = cols[3].Trim();
                p.CellphoneNumber = cols[4].Trim();

                output.Add(p);
            }

            return output;
        }

        /// <summary>
        /// Convert lines from TeamModel.csv file to TeamModel instances.
        /// Each line in the file represents a team.
        /// Info about TeamModels.csv :
        /// - Each team member's id should be an id o a person in the PersonModels.csv file
        /// - Each line of text should respect this rule : 
        ///     id,team name,teamMember1Id|teamMember2Id|...|teamMembernId
        ///     Eg:  3,team1,1|3|5
        /// </summary>
        /// <param name="lines">Lines of text from a csv file</param>
        /// <returns>List of TeamModel objects created from each line of a csv file</returns>
        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string PeopleFileName)
        {
            List<TeamModel> output = new List<TeamModel>();

            List<PersonModel> people = PeopleFileName.FullFilePath().LoadFile().ConvertToPersonModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0].Trim());
                t.TeamName = cols[1].Trim();

                // get the ids of each team member from a team
                string[] personIds = cols[2].Split('|');

                // looking up these people ids.
                // What happens if you have an person Id that you can't find ? blow up with an error
                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                }

                output.Add(t);
            }

            return output;
        }

        /// <summary>
        /// Insert a tournament in the database/text file
        /// Insert all of the prizes ids in the database/text file
        /// Insert all of the teams ids in the database/text file
        /// Example of line in TournamentModels.csv :
        /// id,tournamentName,EntryFee,TeamId1|TeamId2|TeamIdN,PrizeId1|PrizeId2|..|PrizeIdN,MatchupModelId1^MatchupModelId2^MatchupModelId3|MatchupModelId4^MatchupModelId5^MatchupModelId6
        /// </summary>
        ///  <param name="lines">Lines of text from a csv file</param>
        /// <returns>List of Tournament objects created from each line of a csv file</returns>
        public static List<TournamentModel> ConvertToTournamentModels(this List<string> lines, 
                                                                        string teamFileName,
                                                                        string peopleFileName,
                                                                        string prizesFileName)
        {
            List<TournamentModel> output = new List<TournamentModel>();

            // get a list of teams from the text file
            List<TeamModel> teams = teamFileName.FullFilePath().LoadFile().ConvertToTeamModels(peopleFileName);

            // get a list of available prizes from the text file
            List<PrizeModel> prizes = teamFileName.FullFilePath().LoadFile().ConvertToPrizeModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TournamentModel tournamentModel = new TournamentModel();

                tournamentModel.Id = int.Parse(cols[0].Trim());
                tournamentModel.TournamentName = cols[1].Trim();
                tournamentModel.EntryFee = decimal.Parse(cols[2].Trim());

                // get all of the team ids in a tournament
                string[] teamIds = cols[3].Split('|');
                foreach(string teamId in teamIds)
                {
                    // get a team id from Tournametns file and compare it to a all the team ids from the Teams file
                    // => all the teams corresponding to a tournament
                    tournamentModel.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(teamId)).First());
                }

                // get all of the prize ids in a tournament
                string[] prizeIds = cols[4].Split('|');
                foreach (string prizeId in prizeIds)
                {
                    tournamentModel.Prizes.Add(prizes.Where(x => x.Id == int.Parse(prizeId)).First());
                }

                // TODO - Capture rounds information

                output.Add(tournamentModel);
            }

            return output;
        }

        /// <summary>
        /// Create a csv file with each line representing input data about a prize the user has input
        /// </summary>
        /// <param name="models">The PrizeModel list to write to a text file</param>
        /// <param name="fileName">The name of the file where data will be written</param>
        public static void SaveToPrizeFile(this List<PrizeModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach(PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            // if the file already exists, it is overwritten with the new values and the file is closed
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        /// <summary>
        /// Create a csv file with each line representing Person information about that the user has input
        /// </summary>
        /// <param name="models">The PrizeModel list to write to a text file</param>
        /// <param name="fileName">The name of the file where data will be written</param>
        public static void SaveToPeopleFile(this List<PersonModel> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{p.Id},{ p.FirstName },{ p.LastName },{ p.EmailAddress },{ p.CellphoneNumber }");
            }

            // if the file already exists, it is overwritten with the new values and the file is closed
            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        /// <summary>
        /// Create a csv file with each line representing a team created with it's id, name, and team member ids separated by a '|'
        /// </summary>
        /// <param name="models">List to TeamModel objects</param>
        /// <param name="FileName">The file where each TeamModel instance will be saved</param>
        public static void SaveToTeamFile(this List<TeamModel> models, string FileName)
        {
            List<string> lines = new List<string>();

            foreach(TeamModel t in models)
            {
                
                lines.Add($"{ t.Id },{ t.TeamName },{ ConvertPeopleListToString(t.TeamMembers) }");
            }

            File.WriteAllLines(FileName.FullFilePath(), lines);
        }

        /// <summary>
        /// Save each matcup corresponding to a round in a file
        /// Save each round to a file with the corresponding matchup ids
        /// </summary>
        /// <param name="model">Tournament model</param>
        /// <param name="MatchupFile">The file where the matchups will be written to</param>
        /// <param name="MatchupEntryFile">The file where the rounds will be written to</param>
        public static void SaveRoundsToFile(this TournamentModel model, string matchupFile, string matchupEntryFile)
        {
            // Loop through each round
            foreach (List<MatchupModel> round in model.Rounds)
            {
                // Loop through each matchup
                foreach (MatchupModel matchup in round)
                {
                    // Load all of the matchups from file
                    // Get the top id and add one
                    // Store the id
                    // Save the matchup record
                    matchup.SaveMatchupToFile(matchupFile, matchupEntryFile);

                   
                }
            }

            // Get the id for the new matchup and save the record
            // Loop through each entry, get the id and save it.
        }

        // !!! TO UNCOMMENT !!!
        //public static List<MatchupEntryModel> ConvertToMatchupEntryModels(this List<string> lines)
        //{
        //    // id = column 0, TeamCompeting id = column 1, Score = column 2, ParentMatchup = column 3
        //    List<MatchupEntryModel> output = new List<MatchupEntryModel>();

        //    foreach (string line in lines)
        //    {
        //        string[] cols = line.Split(',');

        //        MatchupEntryModel mem = new MatchupEntryModel();
        //        mem.Id = int.Parse(cols[0]);
        //        mem.TeamCompeting = LookupTeamById(int.Parse(cols[1]));
        //        mem.Score = double.Parse(cols[2]);
        //        mem.ParentMatchup = LookupMatchupById(int.Parse(cols[3]));
        //    }

        //}

        // !!! TO UNCOMMENT !!!
        //private static List<MatchupEntryModel> ConvertStringToMatchupEntryModels(string input)
        //{
        //    string[] ids = input.Split('|');
        //    List<MatchupEntryModel> output = new List<MatchupEntryModel>();
        //    List<MatchupEntryModel> entries = GlobalConfig
        //                                      .MatchupEntryFile
        //                                      .FullFilePath()
        //                                      .LoadFile()
        //                                      .ConvertToMatchupEntryModels();

        //    foreach (string id in ids)
        //    {
        //        output.Add(entries.Where(x => x.Id == int.Parse(id)).First());
        //    }

        //    return output;
        //}

        private static TeamModel LookupTeamById(int id)
        {
            List<TeamModel> teams = GlobalConfig
                                    .TeamFile
                                    .FullFilePath()
                                    .LoadFile()
                                    .ConvertToTeamModels(GlobalConfig.PeopleFile);

            return teams.Where(x => x.Id == id).First();
        }

        /// <summary>
        /// Convert lines from a csv file to MatchupModel instances
        /// </summary>
        /// <param name="lines">Lines of text from a csv file</param>
        /// <returns>List of MatchupModel objects created from each line of a csv file</returns>
        //public static List<MatchupModel> ConvertToMatchupModels(this List<string> lines)
        //{
        //    // id=0, entries=1(pipe delmited by id), winner=2, matchupRound=3
        //    List<MatchupModel> output = new List<MatchupModel>();

        //    // In order for the code to work, the content of the file needs to be in the correct order
        //    // and needs to have valid data
        //    foreach (string line in lines)
        //    {
        //        string[] cols = line.Split(',');

        //        MatchupModel p = new MatchupModel();
        //        p.Id = int.Parse(cols[0]);
        //        p.Entries = ConvertStringToMatchupEntryModels(cols[1]);
        //        p.Winner = LookupTeamById(int.Parse(cols[2]));
        //        p.MatchupRound = int.Parse(cols[3]);

        //        output.Add(p);
        //    }

        //    return output;
        //}

        public static void SaveMatchupToFile(this MatchupModel matchup, string matchupFile, string matchupEntryFile)
        {
            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                entry.SaveEntryToFile(matchupEntryFile);
            }
        }


        public static void SaveEntryToFile(this MatchupEntryModel entry,  string matchupEntryFile)
        {
            
        }

        /// <summary>
        /// Create a csv file with each line representing a tournament created with it's id, name, team ids, and matchups
        /// Example of line in TournamentModels.csv :
        /// tournamentId,tournamentName,entryFee,TeamId1|TeamId2|TeamIdN,PrizeId1|PrizeId2|..|PrizeIdN,MatchupModelId1^MatchupModelId2^MatchupModelId3|MatchupModelId4^MatchupModelId5^MatchupModelId6
        /// </summary>
        /// <param name="models">List to TournamentModel objects</param>
        public static void SaveToTournamentFile(this List<TournamentModel> models, string fileName)
        {
            List<string> lines = new List<string>();
            string tournamentEntries;

            foreach (TournamentModel tm in models)
            {
                tournamentEntries = string.Concat($"{ tm.Id },{ tm.TournamentName },{ tm.EntryFee },",
                                                  $"{ ConvertTeamListToString(tm.EnteredTeams) },{ ConvertPrizeListToString(tm.Prizes) }",
                                                  $"{ ConvertRoundListToString(tm.Rounds) }");

                lines.Add(tournamentEntries);
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        /// <summary>
        /// Take a list of PersonModel and return a string of people ids separated by a '|'
        /// </summary>
        /// <param name="people">The list of team members(Person instances)</param>
        /// <returns>People ids separated by a '|'.</returns>
        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";

            if (people.Count <= 0)
            {
                return "";
            }

            foreach (PersonModel p in people)
            {
                output += $"{ p.Id }|";
            }

            // remove the last '|' at the end of the string
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        /// <summary>
        /// Take a list of TeamModel and return a string of team member ids separated by a '|'
        /// </summary>
        /// <param name="teams">The list of teams</param>
        /// <returns>Team member ids separated by a '|'.</returns>
        private static string ConvertTeamListToString(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count <= 0)
            {
                return "";
            }

            foreach (TeamModel t in teams)
            {
                output += $"{ t.Id }|";
            }

            // remove the last '|' at the end of the string
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        /// <summary>
        /// Take a list of PrizeModel and return a string of prize id's separated by a '|'
        /// </summary>
        /// <param name="prizes">The list of teams</param>
        /// <returns>Tournament prize ids separated by a '|'.</returns>
        private static string ConvertPrizeListToString(List<PrizeModel> prizes)
        {
            string output = "";

            if (prizes.Count <= 0)
            {
                return "";
            }

            foreach (PrizeModel p in prizes)
            {
                output += $"{ p.Id }|";
            }

            // remove the last '|' at the end of the string
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        /// <summary>
        /// Take a list of a list of MatchcupModel ids and return a string of MatchupModelId1^MatchupModelId2^MatchupModelId3|MatchupModelId4^MatchupModelId5^MatchupModelId6
        /// </summary>
        /// <param name="rounds">The list of a list of MatchupoModel instances</param>
        /// <returns>Matchups separated in rounds</returns>
        private static string ConvertRoundListToString(List<List<MatchupModel>> rounds)
        {
            // MatchupModelId1^MatchupModelId2^MatchupModelId3|MatchupModelId4^MatchupModelId5^MatchupModelId6

            string output = "";

            if (rounds.Count <= 0)
            {
                return "";
            }

            foreach (List<MatchupModel> listMatchupModel in rounds)
            {
                output += $"{ ConvertMatchupListToString(listMatchupModel) }|";
            }

            // remove the last '|' at the end of the string
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        /// <summary>
        /// Take a list of MatchcupModel ids and return a string of matchup model ids separated by ^
        /// </summary>
        /// <param name="rounds">The list of a list of MatchupoModel instances</param>
        /// <returns>string of matchup ids in a game round, separated by '^'</returns>
        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count <= 0)
            {
                return "";
            }

            foreach (MatchupModel matchupModel in matchups)
            {
                output += $"{ matchupModel.Id }^";
            }

            // remove the last '^' at the end of the string
            output = output.Substring(0, output.Length - 1);

            return output;
        }
    }
}
