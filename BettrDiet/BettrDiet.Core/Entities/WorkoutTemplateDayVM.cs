using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BettrFitSPA.Viewmodels;


namespace BettrFitSPA.Viewmodels
{
    public class WorkoutTemplateDayVM
    {
        public WorkoutTemplateDayVM()
        {
            Sets = new List<WorkoutTemplateSetVM>();
            MuscleGroups = new List<MuscleGroupVM>();
        }
        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public List<MuscleGroupVM> MuscleGroups { get; set; }
        public List<WorkoutTemplateSetVM> Sets { get; set; }
        public byte Day { get; set; }
    }
}
