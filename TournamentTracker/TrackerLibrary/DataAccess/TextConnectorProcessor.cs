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
        /// <param name="models"></param>
        /// <param name="FileName"></param>
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
        /// Take a list of PersonModel and return a string with the people id's separated by a '|'
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
    }
}
