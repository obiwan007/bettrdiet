using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BettrFitSPA.Viewmodels
{
    public class NutritionPlanEntryVM 
    {
        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public short Mahlzeit { get; set; }
        public float Menge { get; set; }
        public float kCalGesamt { get; set; }
        public float Fett { get; set; }
        public float Prot { get; set; }
        public float KH { get; set; }
        public float kJoule { get; set; }
        public string Lebensmittel_id { get; set; }
        public string PlanTag { get; set; }
        public string MengeForEinheit { get; set; }
        public string Einheit { get; set; }
    }
}
