using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class RezeptVM 
    {
        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Username { get; set; }
        public string Name { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsReleased { get; set; }        
        public string FullDescription { get; set; }
        public string Description { get; set; }        
        
        public List<IngredientVM> Ingredients { get; set; }
    }
}
