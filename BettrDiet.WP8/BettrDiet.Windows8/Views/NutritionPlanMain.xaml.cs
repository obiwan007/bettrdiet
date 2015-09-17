using BettrDiet.Core.ViewModels;
using BettrDiet.Windows8.Common;
using BettrFitSPA.Viewmodels;
using Cirrious.MvvmCross.WindowsStore.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Hub-Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=321224 dokumentiert.

namespace BettrDiet.Windows8.Views
{
    /// <summary>
    /// Eine Seite, auf der eine gruppierte Auflistung von Elementen angezeigt wird.
    /// </summary>
    public sealed partial class NutritionPlanMain : MvxStorePage
    {
        public NutritionPlanMain()
        {
            this.InitializeComponent();
        }

        private void listConsumed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var SelectedItems = (this.DataContext as NutritionPlanMainViewModel).SelectedConsumed;
            foreach (var item in e.RemovedItems)
            {
                if (SelectedItems.Contains(item))
                {
                    SelectedItems.Remove(item as LebensmittelConsumedVM);
                }
            }

            foreach (var item in e.AddedItems)
            {
                if (!SelectedItems.Contains(item))
                {
                    SelectedItems.Add(item as LebensmittelConsumedVM);
                }
            }
        }
    }
}
