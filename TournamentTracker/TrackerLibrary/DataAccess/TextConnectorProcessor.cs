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

            // The content of the file needs to be in the correct order
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
        /// Create a csv file with each line representing input data from CreatePrize since the application was launched
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

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }
    }
}
