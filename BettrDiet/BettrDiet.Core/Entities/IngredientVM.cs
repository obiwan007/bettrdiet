using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BettrFitSPA.Viewmodels
{
    public class IngredientVM 
    {
        public string _id { get; set; }
        public string _accessId { get; set; }
    
        public int Id { get; set; }
        public LebensmittelVM Lebensmittel { get; set; }
        public float Menge { get; set; }
        public string Name { get; set; }
    }
}
