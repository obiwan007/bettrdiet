using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class MuscleGroupVM 
    {
        public MuscleGroupVM()
        {
            BelongsTo_id = new List<string>();
            BelongsTo = new List<MuscleGroupVM>();
        }
        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public short Size { get; set; }
        public string Picture { get; set; }
        public string Link { get; set; }
        public string Video { get; set; }
        public string Antagonist_id { get; set; }
        public MuscleGroupVM Antagonist { get; set; }
        public List<string> BelongsTo_id { get; set; }
        public List<MuscleGroupVM> BelongsTo { get; set; }
    }
}
