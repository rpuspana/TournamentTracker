﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
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
            TrackerLibrary.GlobalConfig.InitializeConnections(true, true);

            // pauses this thread with this method
            // don't end this line until the form in it closes
            // if we close this form, even if there are other forms are opened, the application closes
            //Application.Run(new TournamentDashboardForm());

            Application.Run(new CreatePrizeForm());
        }
    }
}
