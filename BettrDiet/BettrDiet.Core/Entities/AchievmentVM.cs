using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class AchievmentVM
    {
        public AchievmentVM()
        {
            DependsOn = new List<string>();
        }
        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string Image { get; set; }
        
        public int Points { get; set; }

        public string Video { get; set; }

        public List<string> DependsOn { get; set; }

        public string MetaData { get; set; }

        public string Category { get; set; }

        public bool IsDeleted { get; set; }

        public bool DoNotify { get; set; }

        public string WorkoutPlanCompleted_id { get; set; }

    }
}
