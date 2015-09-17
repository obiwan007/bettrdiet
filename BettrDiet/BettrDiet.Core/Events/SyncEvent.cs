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
    public class SyncEvent : MvxMessage
    {        
        public SyncEvent(object sender, bool isSyncing) : base(sender)
        {
            IsSyncing = isSyncing;
        }


        public bool IsSyncing { get; set; }
    }
}
