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

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> _availableTeamMembers = GlobalConfig.Connection.GetPersonAll();
        private List<PersonModel> _selectedTeamMembers = new List<PersonModel>();

        public CreateTeamForm()
        {
            InitializeComponent();

            //CreateSampleData();

            WireUpLists();
        }

        private void CreateSampleData()
        {
            _availableTeamMembers.Add(new PersonModel { FirstName = "Tim", LastName = "Corey" });
            _availableTeamMembers.Add(new PersonModel { FirstName = "Sue", LastName = "Storm" });

            _selectedTeamMembers.Add(new PersonModel { FirstName = "Jane", LastName = "Smith" });
            _selectedTeamMembers.Add(new PersonModel { FirstName = "Bill", LastName = "Jones" });
        }

        private void WireUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;
            selectTeamMemberDropDown.DataSource = _availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName"; // FullName is a property in the Person model

            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = _selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PersonModel p = new PersonModel();

                p.FirstName = firstNameValue.Text;
                p.LastName = lastNameValue.Text;
                p.EmailAddress = emailValue.Text;
                p.CellphoneNumber = cellPhoneValue.Text;

                GlobalConfig.Connection.CreatePerson(p);

                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellPhoneValue.Text = "";
            }
            else
            {
                MessageBox.Show("You need to fill all the fields.");
            }
        }

        private bool ValidateForm()
        {
            // TODO - Add validation to the form
            if (firstNameValue.Text.Length == 0)
            {
                return false;
            }

            if (lastNameValue.Text.Length == 0)
            {
                return false;
            }

            if (emailValue.Text.Length == 0)
            {
                return false;
            }

            if (cellPhoneValue.Text.Length == 0)
            {
                return false;
            }

            return true;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;

            _availableTeamMembers.Remove(p);

            _selectedTeamMembers.Add(p);

            WireUpLists();
        }

        private void btnRemoveSelectedMember_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;

            _selectedTeamMembers.Remove(p);

            _availableTeamMembers.Add(p);

            WireUpLists();
        }
    }
}
