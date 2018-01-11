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
        private const string _PrizesFile = "PrizeModels.csv";
        private const string _PeopleFile = "PersonModels.csv";

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

            // Add the new record to the new ID(max + 1)
            prizes.Add(model);

            // Convert the prizes to a list<string>
            // Save the list<string> to the text file
            prizes.SaveToPrizeFile(_PrizesFile);

            return model;
        }
    }
}
