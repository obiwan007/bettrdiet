using BettrFit.Core.Common;
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// Die Elementvorlage "Standardseite" ist unter http://go.microsoft.com/fwlink/?LinkId=234237 dokumentiert.

namespace BettrDiet.WP8.Views
{
    /// <summary>
    /// Eine Standardseite mit Eigenschaften, die die meisten Anwendungen aufweisen.
    /// </summary>
    public sealed partial class Register : MvxPhonePage
    {
        public Register()
        {
            this.InitializeComponent();
        }

        private async void HyperlinkButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var c = CultureHelper.GetCurrentCulture().Substring(0, 2);
            var uri = new Uri("ms-appx:///Assets/AGB_" + c + ".html");
            var options = new Windows.System.LauncherOptions();
            options.DisplayApplicationPicker = false;
            //nav.Navigate(uri);
            var f = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);

            Windows.System.Launcher.LaunchFileAsync(f);
        }
       
        private void StackPanel_BindingValidationError(object sender, System.Windows.Controls.ValidationErrorEventArgs e)
        {
            var sp = sender as StackPanel;
            if (e.Action == ValidationErrorEventAction.Added)
            {

                sp.Children.Insert(0,new TextBlock()
                    {
                        Text = e.Error.ErrorContent.ToString(),
                        Tag = e.Error.Exception,
                        Foreground = new SolidColorBrush(Colors.Red)
                    });
            }
            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                foreach (var a in sp.Children.ToList())
                {
                    var t = a as TextBlock;
                    if (t != null)
                    {
                        if (t.Tag == e.Error.Exception)
                        {
                            sp.Children.Remove(a);
                            break;
                        }
                    }
                }

            }

        }

        void InputBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                if (sender == txtEmail)
                    txtPwd1.Focus();
                else if (sender==txtPwd1)
                    txtPwd2.Focus();
                else if (sender == txtPwd2)
                    txtNick.Focus();
                else if (sender == txtNick)
                    this.Focus();
                
            }
        }        
    }
}
