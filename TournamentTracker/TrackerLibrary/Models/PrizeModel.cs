using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PrizeModel
    {
        /// <summary>
        /// The unique identifier for the prize
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The friendly name for place (second place, first runner up, etc)
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        ///  The numeric identifier for the place (1 for the first place, etc)
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// The fixed amount this place earns or zero if it is not used.
        /// </summary>
        public decimal PrizeAmount { get; set; }

        /// <summary>
        /// The number that represents the percentage of the overall take or
        /// zero if it is not used. The percentage is a fraction of 1
        /// (so 0.5 for 50%)
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel() { }

        public PrizeModel(string placeName, string placeNumber, 
                          string prizeAmount, string prizePercentage )
        {
            PlaceName = placeName;

            // default value for PlaceNumber
            int placeNumberValue = 0;
            int.TryParse(placeNumber, out placeNumberValue);
            PlaceNumber = placeNumberValue;

            // default value for PrizeAmount
            decimal prizeAmountValue = 0;
            decimal.TryParse(prizeAmount, out prizeAmountValue);
            PrizeAmount = prizeAmountValue;

            // default value for PrizePercentage
            double prizePercentageValue = 0;
            double.TryParse(prizePercentage, out prizePercentageValue);
            PrizePercentage = prizePercentageValue;
        }
    }
}
