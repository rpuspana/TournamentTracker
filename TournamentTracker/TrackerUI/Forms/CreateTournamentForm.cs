using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerUI.Forms;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        /// <summary>
        /// Every team stored in the database/file that populates the Select Team Member dropdown list
        /// </summary>
        List<TeamModel> _availableTeams = GlobalConfig.Connection.GetTeamAll();

        /// <summary>
        /// Team that is selected for entering a tournament
        /// </summary>
        List<TeamModel> _selectedTeams = new List<TeamModel>();

        /// <summary>
        /// Prizes available in a tournament
        /// </summary>
        List<PrizeModel> _selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();

            WireUpLists();
        }

        /// <summary>
        /// Populate the select team dropdown with the teams in the database/text file
        /// Populate the tournamentTeamsListBox with the teams selected from the dropdown
        /// Prizes ???
        /// </summary>
        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;
            tournamentTeamsListBox.DataSource = null;
            prizesListBox.DataSource = null;

            // Select Team Member dropdown gets a list with all the available teams
            selectTeamDropDown.DataSource = _availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            // Teams that are selected from the above mentioned list to be part of a tournament
            tournamentTeamsListBox.DataSource = _selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";

            // Prizes that are available fro this tournament
            prizesListBox.DataSource = _selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void entryFeeValue_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel teamModel = (TeamModel)selectTeamDropDown.SelectedItem;

            if (teamModel == null)
            {
                MessageBox.Show("Please select one team from the Select Team list in order to add a team to the tournament",
                                "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _availableTeams.Remove(teamModel);

            _selectedTeams.Add(teamModel);

            WireUpLists();
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            // call the CreatePrizeForm
            // this = the form CreateTournamentForm, and a IPrizeRequester contract fullfiller 
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();
        }

        public void PrizeComplete(PrizeModel model)
        {
            // get back from the CreatePrize form a PrizeModel - complete

            // take that PrizeModel and put it into our list of selected prizes
            _selectedPrizes.Add(model);

            // selectedPrizes listbox needs updating because we added a new prize in the list
            WireUpLists();
        }

        public void TeamComplete(TeamModel model)
        {
            _selectedTeams.Add(model);

            // selectedPrizes listbox needs updating because we added a new prize in the list
            WireUpLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        private void removeSelectedPlayersButton_Click(object sender, EventArgs e)
        {
            TeamModel team = (TeamModel)tournamentTeamsListBox.SelectedItem;

            if (team == null)
            {
                MessageBox.Show("Please select one team from the list and click this button again",
                                "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _selectedTeams.Remove(team);

            _availableTeams.Add(team);

            WireUpLists();
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel prize = (PrizeModel)prizesListBox.SelectedItem;

            if (prize == null)
            {
                MessageBox.Show("Please select one prize from the Prizes list and click this button again",
                                "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _selectedPrizes.Remove(prize);

            WireUpLists();
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            // validate data
            decimal entryFee = 0;
            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text, out entryFee);

            // create the tournament model
            TournamentModel tournamentModel = new TournamentModel();

            // create tournament entry
            tournamentModel.TournamentName = tournamentNameValue.Text;

             if (!feeAcceptable)
             {
                MessageBox.Show("You need to enter a floating point number for the entry fee.",
                               "", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            tournamentModel.EntryFee = entryFee;
            tournamentModel.Prizes = _selectedPrizes;
            tournamentModel.EnteredTeams = _selectedTeams;

            // Create matchups between two teams, tournament rounds, and establish winner
            TournamentLogic.CreateRounds(tournamentModel);

            // Save the tournament's name and entry fee in a database/text file
            // Save all the prizes ids in a database/text file
            // Save all the teams ids participating in the tournament in a database/text file
            GlobalConfig.Connection.CreateTournament(tournamentModel);
        }
    }
}
