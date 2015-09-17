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
    public class LoggedInEvent : MvxMessage
    {        
        public LoggedInEvent(object sender, bool loggedin) : base(sender)
        {
            LoggedIn = loggedin;
        }

        public bool LoggedIn { get; set; }
    }
}
