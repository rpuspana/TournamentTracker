using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        // the user has the option to save in a database of a text file or text files
        // declare getters and setters and initialize list
        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections(DatabaseType db)
        {
            if (db == DatabaseType.Sql)
            {
                // TODO - Set up the SQL Connector properly
                SqlConnector sqlConn = new SqlConnector();
                Connection = sqlConn;
            }
            else if (db == DatabaseType.TextFile)
            {
                // TODO - create the text file connection
                TextConnector textFileConn = new TextConnector();
                Connection = textFileConn;
            }
        }

        /// <summary>
        /// return the value of the connectionString attribute from the <connectionStrings> tag of App.config
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
