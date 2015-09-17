using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class NutritionPlanFavoriteVM
    {
        public string _id { get; set; }
        public string _accessId { get; set; }

        public string User_id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Plan_id { get; set; }
        public short Mahlzeit { get; set; }
        public bool IsPublic { get; set; }
        public List<LebensmittelConsumedVM> Lebensmittel { get; set; }

    }
}
