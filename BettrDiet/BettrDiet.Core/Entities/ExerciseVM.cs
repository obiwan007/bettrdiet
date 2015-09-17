using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BettrFitSPA.Viewmodels.Training;

namespace BettrFitSPA.Viewmodels.Training
{
    public class ExerciseVM 
    {

        public string _id { get; set; }
        public string _accessId { get; set; }

        public ExerciseVM()
        {
            Equipment = new List<EquipmentVM>();
            ExerciseTypes = new List<ExerciseTypeVM>();
            Pictures = new List<PictureVM>();
            ExerciseMuscleEffectTypes = new List<ExerciseMuscleEffectTypeVM>();
        }


        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Link { get; set; }
        public string Video { get; set; }
        public byte DifficultyLevel { get; set; }
        public byte FitnessLevel { get; set; }
        public bool IsEntryLevel { get; set; }
        public bool IsObsolete { get; set; }
        public List<EquipmentVM> Equipment { get; set; }
        public List<ExerciseTypeVM> ExerciseTypes { get; set; }
        public List<PictureVM> Pictures { get; set; }
        public List<ExerciseMuscleEffectTypeVM> ExerciseMuscleEffectTypes { get; set; }

        public bool IsReference { get; set; }
        public byte Goal_Endurance { get; set; }
        public byte Goal_FatLoss { get; set; }
        public byte Goal_Muscle { get; set; }
        public byte Goal_Posture { get; set; }
        public byte Goal_Strength { get; set; }
        public byte Priority { get; set; }
    }
}
