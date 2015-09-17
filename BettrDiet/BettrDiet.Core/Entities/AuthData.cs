using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class UserRole
    {
        public string Name { get; set; }
    }
    public enum Features
    {
        Administrator,
        Nutrition,
        DailyData,
        Author
    }

    public class AuthData
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthenticatedToken { get; set; }
        public string Ret { get; set; }
        public int Code { get; set; }
        public int ProviderId { get; set; }
        public string DeviceType { get; set; }
        public string DeviceId { get; set; }
        public string SubscriptionType { get; set; }
        public string SubscriptionId { get; set; }
        public List<Features> UserFeatures { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }

}
