using System;
using System.Collections.Generic;
using System.Linq;

namespace BettrFitSPA.Viewmodels
{
    public class NutritionPlanVM
    {

        public string _id { get; set; }
        public string _accessId { get; set; }

        public string user_id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<NutritionPlanDayVM> Days { get; set; }

        public bool IsPrivate { get; set; }        

        public DateTime StartDate { get; set; }

        public string PlanType { get; set; }

        public bool IsActivePlan { get; set; }
    }

}