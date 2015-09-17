using BettrFitSPA.Viewmodels.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class BlogPostVM 
    {

        public string _id { get; set; }
        public string _accessId { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime PublishTime { get; set; }
        public DateTime DePublishTime { get; set; }
        public bool IsReleased { get; set; }
        public string Text { get; set; }
        public string Teaser { get; set; }
        public string Headline { get; set; }
        public string Author_id { get; set; }
        public string Author { get; set; }
        public List<string> BlogKeyword { get; set; }
        public List<BlogCommentEntryVM> BlogComment { get; set; }
        public string DestinationId { get; set; }
        public bool IsNews { get; set; }
        public bool IsPromoted { get; set; }

        public string Keywords { get; set; }
        public List<PictureVM> Pictures { get; set; }
    }        
}
