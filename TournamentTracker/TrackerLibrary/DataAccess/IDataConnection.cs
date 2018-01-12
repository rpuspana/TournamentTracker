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
        /// Gets all the Person instances from the database or from a text file
        /// </summary>
        /// <param name="model">The person information.</param>
        /// <returns>A list of PesonModel instances from the database or fro a text file</returns>
        List<PersonModel> GetPersonAll();
    }
}
