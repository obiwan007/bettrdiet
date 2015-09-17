using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class AchievmentHistoryVM
    {

        public string _id { get; set; }
        public string _accessId { get; set; }


        public string Comments { get; set; }

        public DateTime Date { get; set; }

        public AchievmentVM Achievment { get; set; }
    }
}
