using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class PlannedExerciseGroupVM
    {
        public PlannedExerciseGroupVM()
        {
            PlannedExercises = new List<PlannedExerciseVM>();
        }
        public string _id { get; set; }
        public string _accessId { get; set; }
        
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Index { get; set; }

        public List<PlannedExerciseVM> PlannedExercises { get; set; }
        
    }
}
