using System;
using System.Collections.Generic;
using System.Linq;

namespace BettrFitSPA.Viewmodels
{
    public class PlannedWorkoutVM
    {

        public PlannedWorkoutVM()
        {
            PlannedWorkoutDays = new List<PlannedWorkoutDayVM>();
        }

       public string _id { get; set; }
       public string _accessId { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public string Name { get; set; }

        public DateTime PlannedFor { get; set; }

        public bool IsDeleted { get; set; }

        public short TrainingCount { get; set; }

        public string _WorkoutTemplate_id { get; set; }
        
        public List<PlannedWorkoutDayVM> PlannedWorkoutDays { get; set; }
        public string User_id { get; set; }


        public bool IsPublic { get; set; }
    }
    
}