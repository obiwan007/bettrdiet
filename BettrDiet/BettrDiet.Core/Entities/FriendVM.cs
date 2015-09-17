using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BettrFitSPA.Models
{
    public class FriendVM 
    {
        public FriendVM()
        {
            
        }

        public string _id { get; set; }
        public string _accessId { get; set; }

        public string User_id { get; set; }
        public string Nickname { get; set; }

        public string Notes { get; set; }

        public bool RequestSent { get; set; }
        public bool RequestAccepted { get; set; }

        public DateTime InvitationDate { get; set; }

        public string Group { get; set; }

        public string Image { get; set; } 
    }
}
