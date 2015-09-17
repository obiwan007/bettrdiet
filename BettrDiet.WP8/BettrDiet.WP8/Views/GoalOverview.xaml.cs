using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;


using Microsoft.Phone.Reactive;
using System.Diagnostics;
using Telerik.Charting;
using Cirrious.MvvmCross.WindowsPhone.Views;


namespace BettrDiet.WP8.Views
{
    public partial class GoalOverview : MvxPhonePage
    {

        public GoalOverview()
        {
            InitializeComponent();

        }

        private void ChartTrackBallBehavior_TrackInfoUpdated_1(object sender, Telerik.Windows.Controls.TrackBallInfoEventArgs e)
        {
            CategoricalDataPoint dataPoint = e.Context.ClosestDataPoint.DataPoint as CategoricalDataPoint;
            DateTime date = (DateTime)dataPoint.Category;
            e.Context.ClosestDataPoint.DisplayHeader = "Weight on "+date.ToShortDateString();
            e.Context.ClosestDataPoint.DisplayContent=dataPoint.Value+" kg";
        }

        private void chart_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            //var p=chartWeight.PanOffset;
            
            //Debug.WriteLine("X:" + p.X + " Y:" + p.Y);

        }

        private void ChartTrackBallBehavior_TrackInfoUpdatedFat(object sender, Telerik.Windows.Controls.TrackBallInfoEventArgs e)
        {
            CategoricalDataPoint dataPoint = e.Context.ClosestDataPoint.DataPoint as CategoricalDataPoint;
            DateTime date = (DateTime)dataPoint.Category;
            e.Context.ClosestDataPoint.DisplayHeader = "Fat % on " + date.ToShortDateString();
            e.Context.ClosestDataPoint.DisplayContent = Math.Round((decimal)dataPoint.Value,1) + " %";

        }
       
    }
}