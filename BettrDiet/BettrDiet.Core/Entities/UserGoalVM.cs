using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BettrFitSPA.Viewmodels;

namespace BettrFitSPA.Viewmodels.User
{
    public class UserGoalVM 
    {
        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Comment { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? DestinationDate { get; set; }

        public float? FatP { get; set; }

        public byte Goal_Endurance { get; set; }

        public byte Goal_FatLoss { get; set; }

        public byte Goal_Muscle { get; set; }

        public byte Goal_Posture { get; set; }

        public byte Goal_Strength { get; set; }

        public float? MuscleP { get; set; }

        public float? WaterP { get; set; }

        public float? Weight { get; set; }

        public float? Abdomen { get; set; }

        public float? Biceps_L { get; set; }

        public float? Biceps_R { get; set; }

        public float? Calf_L { get; set; }

        public float? Calf_R { get; set; }

        public float? Chest { get; set; }

        public float? Forearm_L { get; set; }

        public float? Forearm_R { get; set; }

        public float? Hips { get; set; }

        public float? Neck { get; set; }

        public float? Shoulders { get; set; }

        public float? Thigh_L { get; set; }

        public float? Thigh_R { get; set; }

        public float? Waist { get; set; }

        public float? Wrist_L { get; set; }

        public float? Wrist_R { get; set; }

        public short? WorkoutDays { get; set; }

        public short? WorkoutMinutes { get; set; }

        public bool IsActive { get; set; }
        
        public bool IsChanged { get; set; }

    }
}
