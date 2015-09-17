using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Common.Models;
using Cirrious.MvvmCross.WindowsPhone.Views;
using System.ServiceModel;
using BettrDiet.Core.Common;
using BettrFit.Core.Common;
using BettrDiet.Core.ViewModels;

namespace BettrDiet.WP8.Views
{
    public partial class NutritionPlanCreate : MvxPhonePage
    {
        // Constructor
        public NutritionPlanCreate()
        {
            InitializeComponent();

			//Shows the trial reminder message, according to the settings of the TrialReminder.
            //(App.Current as App).trialReminder.Notify();

            ////Shows the rate reminder message, according to the settings of the RateReminder.
            //(App.Current as App).rateReminder.Notify(); 

        }
    }
}
