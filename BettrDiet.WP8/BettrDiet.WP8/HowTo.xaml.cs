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
using BettrDiet.WP8.ViewModels;

namespace BettrDiet.WP8
{
    public partial class HowTo : PhoneApplicationPage
    {
        public HowTo()
        {
            InitializeComponent();
            this.DataContext = new MainDataViewModel();
        }

		private void LeftArrow_Click(object sender, EventArgs e)
        {
            this.slideView.MoveToPreviousItem();
        }

        private void RightArrow_Click(object sender, EventArgs e)
        {
            this.slideView.MoveToNextItem();
        }
    }
}
