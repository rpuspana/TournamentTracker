using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        // contract for a method
        // everythin in here is public because of the access modifier of the class
        PrizeModel CreatePrize(PrizeModel model);
    }
}
