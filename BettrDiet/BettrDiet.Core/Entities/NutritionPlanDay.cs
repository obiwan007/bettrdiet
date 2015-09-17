using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class NutritionPlanDayVM
    {
        public string _id { get; set; }
        public string _accessId { get; set; }

        public static bool IncludeIngredients = true;
        
        public NutritionPlanDayVM()
        {

            Date = DateTime.Now;
            Name = "New Plan Day";
            Description = string.Empty;
        }

        

        public string Name { get; set; }

        public string Description { get; set; }

        public List<NutritionPlanEntryVM> Entries { get; set; }


        public DateTime Date { get; set; }

        public int DayOfWeekIndex { get; set; }

        public int WeekIndex { get; set; }

        public int NutritionPlanId { get; set; }

        public Plan1PointsVM Plan1PointsSum { get; set; }

    }
}
