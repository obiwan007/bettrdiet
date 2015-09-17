using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class ExerciseMuscleEffectTypeVM 
    {
        public string _id { get; set; }
        public string _accessId { get; set; }


        public short Effect { get; set; }
        public MuscleGroupVM MuscleGroup { get; set; }
    }
}
