﻿using Cirrious.MvvmCross.Plugins.Messenger;
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
    public class EmailEvent : MvxMessage
    {        
        public EmailEvent(object sender) : base(sender)
        {
            
        }
    }
}
