using Cirrious.MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettrDiet.Core.Events
{
    /// <summary>
    /// Event for signalling if Loggedin or LoggedOut
    /// </summary>
    public class NetworkEvent : MvxMessage
    {
        public NetworkEvent(object sender, bool isAvailable)
            : base(sender)
        {
            IsAvailable = isAvailable;
        }


        public bool IsAvailable { get; set; }
    }
}
