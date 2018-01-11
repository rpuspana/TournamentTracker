using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnection
    {
        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                var p = new DynamicParameters();

                p.Add("FirstName", model.FirstName);
                p.Add("LastName", model.LastName);
                p.Add("EmailAddress", model.EmailAddress);
                p.Add("CellphoneNumber", model.CellphoneNumber);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // specific for operations where nothing is returned from the database
                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                // the model will have the Id = the id of the row just inserted in the Prizes table
                // that's wht id is an output parameter
                model.Id = p.Get<int>("@id");

                return model;
            }
        }

        /// TODO - Make the CreatePrize method actually save to the database
        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including hte unique identifier.</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {

            // connect to SQl Server
            // when reaching the } the connection gets destroyed properly preventing memory leaks
            // *** back in the day, in case of errors or exceptions the connection would remain open
            // many errors would cause many opened connections to SQL Server, thus SQL Server would get slower and slower
            // and my application will become slower and slower. Memory usage will go through the roof
            // *** this problem was fixed by the using statement. Even on exceptions, close down the database connection
            // *** open the connection every single time
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                var p = new DynamicParameters();

                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // specific for operations where nothing is returned from the database
                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                // the model will have the Id = the id of the row just inserted in the Prizes table
                // that's wht id is an output parameter
                model.Id = p.Get<int>("@id");

                return model;
            }
        }
    }
}
