using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels.Training
{
    public class TrainingExerciseGroupVM 
    {
        public TrainingExerciseGroupVM()
        {
            TrainingExercises = new List<TrainingExerciseVM>();
        }

        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public byte Index { get; set; }
        public List<TrainingExerciseVM> TrainingExercises { get; set; }


        
    }
}
