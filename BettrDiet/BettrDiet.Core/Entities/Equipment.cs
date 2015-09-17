using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels.Training
{
    public class EquipmentVM 
    {
        public EquipmentVM()
        {
            EquipmentTypes = new List<EquipmentTypeVM>();
        }

        public string _id { get; set; }
        public string _accessId { get; set; }


        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Producer { get; set; }
        public string Link { get; set; }
        public string Video { get; set; }
        public List<EquipmentTypeVM> EquipmentTypes { get; set; }

    }
}
