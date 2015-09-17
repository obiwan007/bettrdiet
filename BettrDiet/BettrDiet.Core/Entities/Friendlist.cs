using BettrFitSPA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class FriendlistVM
    {
        public FriendlistVM()
        {
            Friends = new List<FriendVM>();
        }

        public string _id { get; set; }
        public string _accessId { get; set; }

        
        public string Name { get; set; }
        
        public string Notes { get; set; }

        public string Image { get; set; }

        public List<FriendVM> Friends { get; set; }

        public bool CanSeeMyWorkouts { get; set; }
        public bool CanSeeMyStatus { get; set; }
        public bool CanSeeMyDetails { get; set; }
        public bool CanSeeMyPictures { get; set; }

    }
}
