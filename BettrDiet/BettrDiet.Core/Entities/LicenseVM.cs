using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BettrFitSPA.Viewmodels
{
    public class LicenseVM
    {
        public string _id { get; set; }
        public string _accessId { get; set; }
        public string Receipt { get; set; }
        public string LicenseType { get; set; }
        public string OSType { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public bool IsValid { get; set; }
    }
}
