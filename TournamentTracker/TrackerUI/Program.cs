using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerUI.Forms;

namespace TrackerUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize the database connections
            //GlobalConfig.InitializeConnections(DatabaseType.Sql);

            // Write data to a text file
            GlobalConfig.InitializeConnections(DatabaseType.TextFile);

            // pauses this thread with this method
            // don't end this line until the form in it closes
            // if we close this form, even if there are other forms are opened, the application closes
            //Application.Run(new TournamentDashboardForm());

            //Application.Run(new CreatePrizeForm());

            Application.Run(new CreateTeamForm());
        }
    }
}
