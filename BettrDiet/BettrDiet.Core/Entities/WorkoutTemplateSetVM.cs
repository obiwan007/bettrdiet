using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BettrFitSPA.Viewmodels;
using BettrFitSPA.Viewmodels.Training;


namespace BettrFitSPA.Viewmodels
{
    public class WorkoutTemplateSetVM
    {
        public WorkoutTemplateSetVM()
        {
            MuscleGroups = new List<MuscleGroupVM>();
            MuscleGroups_id = new List<string>();
            Exercises = new List<ExerciseVM>();
            Exercises_id = new List<string>();

        }

        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Name { get; set; }
        public List<MuscleGroupVM> MuscleGroups { get; set; }
        public List<ExerciseVM> Exercises { get; set; }
        
        public short DurationOfExercise { get; set; }

        public string ExerciseTag { get; set; }

        public byte Index { get; set; }

        public byte PercentOfOneRep { get; set; }

        public short RelaxationTime { get; set; }

        public byte RepeatCount { get; set; }

        public byte Reps { get; set; }
        
        public byte Cadence_Contraction { get; set; }

        public byte Cadence_Pause1 { get; set; }

        public byte Cadence_Pause2 { get; set; }

        public byte Cadence_Relaxation { get; set; }

        public string Description { get; set; }

        public List<string> MuscleGroups_id { get; set; }

        public List<string> Exercises_id { get; set; }
    }
}
