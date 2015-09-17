using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BettrFitSPA.Viewmodels;


namespace BettrFitSPA.Viewmodels.User
{
    public class UserDailyVM
    {

        public string _id { get; set; }
        public string _accessId { get; set; }

        
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        public float Weight { get; set; }
        public float FatP { get; set; }
        public float MuscleP { get; set; }
        public float WaterP { get; set; }
        public float Height { get; set; }
        public float Calories { get; set; }

        public int FitnessLevel { get; set; }

        public float? Abdomen { get; set; }

        public float? Biceps_L { get; set; }

        public float? Biceps_R { get; set; }

        public float? Calf_R { get; set; }

        public float? Calf_L { get; set; }

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



        public string User_id { get; set; }





        public bool IsChanged { get; set; }
        public float FatKg
        {
            get { return (float)Math.Round(FatP * Weight / 100, 1); }
        }

        public float MuscleKg
        {
            get { return (float)Math.Round(MuscleP * Weight / 100, 1); }
        }

        public float WaterKg
        {
            get { return (float)Math.Round(WaterP * Weight / 100, 1); }
        }

        public string DateString
        {
            get { return Date.ToString(); }
        }       

    }
}
