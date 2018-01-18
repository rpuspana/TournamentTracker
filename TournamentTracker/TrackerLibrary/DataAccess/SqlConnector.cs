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
        private const string dbName = "Tournaments";
        
        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(dbName)))
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

        /// Make the CreatePrize method actually save to the database
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
            using (IDbConnection conn = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(dbName)))
            {
                var p = new DynamicParameters();

                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // specific for operations where nothing is returned from the database
                conn.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                // the model will have the Id = the id of the row just inserted in the Prizes table
                // that's wht id is an output parameter
                model.Id = p.Get<int>("@id");

                return model;
            }
        }

        /// <summary>
        /// Insert every team member from a team in the TeamMembers table
        /// </summary>
        /// <param name="model">The team information</param>
        /// <returns>A Team model</returns>
        public TeamModel CreateTeam(TeamModel model)
        {
            using (IDbConnection conn = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(dbName)))
            {
                // insert the new team in the Teams table
                var p = new DynamicParameters();

                p.Add("@TeamName", model.TeamName);
                p.Add("@id", 0, DbType.Int32, ParameterDirection.Output);

                conn.Execute("dbo.spTeamsInsert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");

                // insert each team member(Person instance) in the TeamMembers table
                foreach (PersonModel teamMember in model.TeamMembers)
                {
                    p = new DynamicParameters();

                    p.Add("@TeamId", model.Id);
                    p.Add("@PersonId", teamMember.Id);

                    conn.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);
                }

                return model;
            }
        }

        /// <summary>
        /// Gets all Person data from the Tournaments database
        /// </summary>
        /// <param name="model">The person information.</param>
        /// <returns>A list of PesonModel instances from the Tournaments database</returns>
        public List<PersonModel> GetPersonAll()
        {
            List<PersonModel> output;

            using (IDbConnection conn = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(dbName)))
            {
                output = conn.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }

            return output;
        }

        /// <summary>
        /// Get all the teams from the database or from a text file
        /// </summary>
        /// <returns>A list of teams</returns>
        public List<TeamModel> GetTeamAll()
        {
            List<TeamModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(dbName)))
            {
                output = connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();

                foreach(TeamModel team in output)
                {
                    var p = new DynamicParameters();

                    p.Add("@TeamId", team.Id);

                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                }

            }

            return output;
        }
    }
}
