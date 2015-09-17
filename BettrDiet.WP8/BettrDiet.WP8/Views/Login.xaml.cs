using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Cirrious.MvvmCross.WindowsPhone.Views;

namespace BettrDiet.WP8.Views
{
    public partial class Login : MvxPhonePage
    {
        public Login()
        {
            InitializeComponent();
        }
       
        private void PhoneApplicationPage_Loaded_1(object sender, RoutedEventArgs e)
        {
            txtEmail.Focus();
        }

    }
}