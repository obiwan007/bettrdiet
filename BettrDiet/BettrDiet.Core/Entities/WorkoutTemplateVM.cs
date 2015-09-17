using System;
using System.Collections.Generic;
using System.Linq;

namespace BettrFitSPA.Viewmodels
{
    public class WorkoutTemplateVM 
    {
        public WorkoutTemplateVM()
        {
            WorkoutTemplateDays = new List<WorkoutTemplateDayVM>();
        }

            public string _id { get; set; }
            public string _accessId { get; set; }


            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsEntryLevel { get; set; }
            public byte Fitnesslevel { get; set; }
            public byte Goal_Endurance { get; set; }
            public byte Goal_FatLoss { get; set; }
            public byte Goal_Muscle { get; set; }
            public byte Goal_Posture { get; set; }
            public byte Goal_Strength { get; set; }

            public List<WorkoutTemplateDayVM> WorkoutTemplateDays { get; set; }

            public bool IsTest { get; set; }

            public string Testpage { get; set; }

            public short TrainingCount { get; set; }

            public short WorkoutMinutes { get; set; }

            public short WorkoutsCountFrom { get; set; }

            public short WorkoutsCountTo { get; set; }
    }
    
}