using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    using BettrFitSPA.Viewmodels.Food;

    public class LebensmittelConsumedRangeVM 
    {
        public LebensmittelConsumedRangeVM()
        {
            Days=new List<LebensmittelConsumedDayVM>();
        }
        public List<LebensmittelConsumedDayVM> Days { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }

    public class LebensmittelConsumedDayVM
    {
        public LebensmittelConsumedDayVM()
        {
            Daytimes=new List<LebensmittelConsumedDaytimeVM>();
        }
        public List<LebensmittelConsumedDaytimeVM> Daytimes { get; set; }
        public DateTime Date { get; set; }
        public SummaryData Summary { get; set; }
    }

    public class LebensmittelConsumedDaytimeVM
    {
        public LebensmittelConsumedDaytimeVM()
        {
            Consumed=new List<LebensmittelConsumedVM>();
        }
        public List<LebensmittelConsumedVM> Consumed { get; set; }
        public DateTime Date { get; set; }
        public int Mahlzeit { get; set; }
    }
}
