using Cirrious.MvvmCross.WindowsStore.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Standardseite" ist unter http://go.microsoft.com/fwlink/?LinkId=234237 dokumentiert.

namespace BettrDiet.Windows8.Views
{
    /// <summary>
    /// Eine Standardseite mit Eigenschaften, die die meisten Anwendungen aufweisen.
    /// </summary>
    public sealed partial class MainView : MvxStorePage
    {
        private double previousWidth;
        private double previousHeight;
        private Thickness previousMargin;

        public MainView()
        {
            this.InitializeComponent();
            this.SizeChanged += VerticalHubPage_SizeChanged;
        }
        void VerticalHubPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < 800)
            {
                mainhub.Orientation = Orientation.Vertical;
                ScrollViewer.SetVerticalScrollMode(mainhub, ScrollMode.Enabled);
                ScrollViewer.SetHorizontalScrollMode(mainhub, ScrollMode.Disabled);
                ScrollViewer.SetVerticalScrollBarVisibility(mainhub, ScrollBarVisibility.Auto);
                ScrollViewer.SetHorizontalScrollBarVisibility(mainhub, ScrollBarVisibility.Disabled);

                HeroSection.Width = e.NewSize.Width - 50;
                HeroSection.Height = 300;
                HeroSection.Margin = new Thickness(50.0, 150.0, 0, 0);

            }
            else
            {
                mainhub.Orientation = Orientation.Horizontal;
                ScrollViewer.SetVerticalScrollMode(mainhub, ScrollMode.Disabled);
                ScrollViewer.SetHorizontalScrollMode(mainhub, ScrollMode.Enabled);
                ScrollViewer.SetVerticalScrollBarVisibility(mainhub, ScrollBarVisibility.Disabled);
                ScrollViewer.SetHorizontalScrollBarVisibility(mainhub, ScrollBarVisibility.Auto);

                HeroSection.Width = previousWidth;
                HeroSection.Height = previousHeight;
                HeroSection.Margin = previousMargin;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            previousWidth = HeroSection.Width;
            previousHeight = HeroSection.Height;
            previousMargin = HeroSection.Margin;
        }

        //private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        //{
        //    var vm=(DataContext as MainPageViewModel);
        //    vm.ResetPwdCommand.Execute();
        //}
    }
}
