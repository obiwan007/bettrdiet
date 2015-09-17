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
    public class PopupEvent : MvxMessage
    {
        
        public PopupEvent(object sender, string message, string header) : base(sender)
        {
            Message = message;
            Header = header;
        }


        public string Message { get; set; }
        public string Header { get; set; }
    }
}
