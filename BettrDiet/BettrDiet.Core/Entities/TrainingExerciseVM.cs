using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels.Training
{
    public class TrainingExerciseVM 
    {

        public TrainingExerciseVM()
        {
            Pictures = new List<PictureVM>();
        }
        public string _id { get; set; }
        public string _accessId { get; set; }

        public short DefaultReps { get; set; }
        public short DefaultRestTime { get; set; }
        public short DefaultTime { get; set; }
        public short DefaultTime1 { get; set; }
        public short DefaultTime2 { get; set; }
        public short DefaultTime3 { get; set; }
        public double DefaultWeight { get; set; }
        public byte Index { get; set; }

        public string Comments { get; set; }

        public bool Completed { get; set; }

        public DateTime CompletionDate { get; set; }

        public byte Rating { get; set; }

        public short UsedReps { get; set; }

        public short UsedTime { get; set; }

        public double UsedWeight { get; set; }

        public string ExerciseName { get; set; }

        public string Exercise_id { get; set; }

        public double OneRepMax { get; set; }

        public string DoneExercise_id { get; set; }

        public string User_id { get; set; }

        public List<PictureVM> Pictures { get; set; }

        public string ExerciseDesc { get; set; }

        public bool IsFirst { get; set; }
    }
}
