using System;
using System.Collections.Generic;
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
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

        public static void InitializeConnections(bool database, bool textFiles)
        {
            if (database)
            {
                // TODO - Set up the SQL Connector properly
                SqlConnector sqlConnector = new SqlConnector();
                Connections.Add(sqlConnector);
            }
            
            if (textFiles)
            {
                // TODO - create the text file connection
                TextConnector textConnector = new TextConnector();
                Connections.Add(textConnector);
            }
        }
    }
}
