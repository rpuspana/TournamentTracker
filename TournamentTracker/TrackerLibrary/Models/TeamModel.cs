using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TeamModel
    {
        /// <summary>
        /// The list of members of a team
        /// </summary>
        // when the class' constructor is used TeamMembers = new List<Person>();
        // C# version 6 syntax
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();

        public string TeamName { get; set; }
    }
}
