using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class PlannedExerciseVM
    {
        public PlannedExerciseVM()
        {

        }
        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Description { get; set; }
        public short DefaultReps { get; set; }
        public short DefaultRestTime { get; set; }
        public short DefaultTime { get; set; }
        public short DefaultTime1 { get; set; }
        public short DefaultTime2 { get; set; }
        public short DefaultTime3 { get; set; }
        public double DefaultWeight { get; set; }
        public string Exercise_id { get; set; }
        public byte Index { get; set; }
        //public PlannedWorkout WorkoutPlan { get; set; }
    }
}
