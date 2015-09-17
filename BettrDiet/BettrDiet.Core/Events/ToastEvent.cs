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
    public class KalorienUpdatedEvent : MvxMessage
    {

        public KalorienUpdatedEvent(object sender, string cals)
            : base(sender)
        {
            Calories = cals;
        }

        public string Calories { get; set; }
    }
}
