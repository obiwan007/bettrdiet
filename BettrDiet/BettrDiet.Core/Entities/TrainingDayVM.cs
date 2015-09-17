using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BettrFitSPA.Viewmodels;


namespace BettrFitSPA.Viewmodels.Training
{
    public class TrainingDayVM 
    {
        public TrainingDayVM()
        {
            TrainingExerciseGroups = new List<TrainingExerciseGroupVM>();
        }

        public string _id { get; set; }
        public string _accessId { get; set; }

        public List<TrainingExerciseGroupVM> TrainingExerciseGroups { get; set; }

        public string Comments { get; set; }
        public string Name { get; set; }

        public byte Rating { get; set; }

        public DateTime TrainingDate { get; set; }

        public bool WorkoutCompleted { get; set; }

        public string PlannedWorkoutDay_id { get; set; }
        public string PlannedWorkout_id { get; set; }

        public bool IsDeleted { get; set; }

        public string User_id { get; set; }

        //public List<Picture> Pictures;

        

    }
}
