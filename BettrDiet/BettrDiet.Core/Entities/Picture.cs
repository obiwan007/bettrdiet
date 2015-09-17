using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels.Training
{
    public class PictureVM 
    {
        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Author { get; set; }
        
        public byte Index { get; set; }
        public string Url { get; set; }
    }
}
