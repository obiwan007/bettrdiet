using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class PlannedWorkoutDayVM
    {
        public PlannedWorkoutDayVM()
        {
            PlannedExerciseGroups = new List<PlannedExerciseGroupVM>();
        }
        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public byte Day { get; set; }        

        public List<PlannedExerciseGroupVM> PlannedExerciseGroups { get; set; }

        
        public DateTime PlannedFor { get; set; }

        public string PlannedWorkout_id { get; set; }
    }
}
