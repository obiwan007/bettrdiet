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
using Cirrious.MvvmCross.WindowsPhone.Views;
using Microsoft.Phone.Info;
using System.Xml.Linq;

namespace BettrDiet.WP8.Views
{
    public partial class About : MvxPhonePage
    {
        public About()
        {
            InitializeComponent();
        }

        private void MvxPhonePage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // These are TextBlock controls that are created in the page’s XAML file      
                txtMemory.Text = "Mem: C:" + DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage").ToString()
                    + " / P:" + DeviceExtendedProperties.GetValue("ApplicationPeakMemoryUsage").ToString()
                    + " / T:" + DeviceExtendedProperties.GetValue("DeviceTotalMemory").ToString();


                XDocument doc = XDocument.Load("WMAppManifest.xml");
                var app = doc.Root
                                  .Element("App");
                var vers = app.Attribute("Version");

                //PackageVersion version = Package.Current.Id.Version;
                txtVersion.Text = "Version: " + vers.Value;

            }
            catch (Exception ex)
            {
                txtMemory.Text = ex.Message;
            }
        }
    }
}
