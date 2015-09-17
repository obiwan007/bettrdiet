using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class MessageVM 
    {
        public MessageVM()
        {
            Replies = new List<string>();
        }

        public string _id { get; set; }
        public string _accessId { get; set; }

        public string Author_id { get; set; }
        
        public string AuthorNickname { get; set; }

        public string Destination_id { get; set; }

        public string Subject { get; set; }

        public bool IsRead { get; set; }

        public bool IsSent { get; set; }

        public DateTime ReadTime { get; set; }

        public string Text { get; set; }

        public byte Type { get; set; }

        public DateTime WriteTime { get; set; }

        public List<string> Replies { get; set; }
    }
}
