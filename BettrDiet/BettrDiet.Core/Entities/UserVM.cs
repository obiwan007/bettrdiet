using System;
using System.Collections.Generic;
using System.Linq;


namespace BettrFitSPA.Viewmodels.User
{
    public class UserVM 
    {

        public UserVM()
        {
            Equipment_ids = new List<string>();
            EquipmentType_ids = new List<string>();
            UserGoals = new List<UserGoalVM>();
            Archievments = new List<AchievmentHistoryVM>();
            //Archivments = new List<AchievmentVM>();
        }

        public string _id { get; set; }
        public string _accessId { get; set; }



        public string Name { get; set; }
        public string Lastname { get; set; }        
        public string Username { get; set; }
        public string Nickname { get; set; }
        //public string FacebookID { get; set; }
        //public string LiveID { get; set; }        
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
        public decimal Height { get; set; }
        public decimal CurrentWeight { get; set; }
        public byte BodyType { get; set; }
        public List<string> EquipmentType_ids { get; set; }
        public List<string> Equipment_ids { get; set; }
        //public string SkypeId { get; set; }
        public string Studio { get; set; }
        public string Location { get; set; }
        public List<UserGoalVM> UserGoals { get; set; }
        //public Guid ValidationToken { get; set; }
        //public bool IsDeleted { get; set; }
        //public bool IsValidated { get; set; }
        //public string OpenID { get; set; }
        //public int ValidatedLogins { get; set; }
        //public int UnvalidatedLogins { get; set; }        
        //public DateTime LastActivity { get; set; }
        public DateTime MemberSince { get; set; }
        public DateTime LastLogin { get; set; }

        //public PlannedWorkoutVM CurrentPlannedWorkout { get; set; }
        public string AboutMe { get; set; }
        public string Hobbies { get; set; }
        public string Motto { get; set; }
        public List<AchievmentHistoryVM> Archievments { get; set; }

        public bool AcceptedAGB { get; set; }
        public DateTime AcceptedAGBDate { get; set; }

        public bool IsHealthVaultUser { get; set; }

        public bool IsChanged { get; set; }

        public string Token { get; set; }
    }
}
