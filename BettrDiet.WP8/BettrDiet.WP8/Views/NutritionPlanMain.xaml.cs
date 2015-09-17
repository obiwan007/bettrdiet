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
using BettrFitSPA.Viewmodels;

namespace BettrDiet.WP8.Views
{
    public partial class NutritionPlanMain : MvxPhonePage
    {
        private NutritionPlanMainViewModel model;
        // Constructor
        public NutritionPlanMain()
        {
            InitializeComponent();

            
        }

        private void listConsumed_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            model = DataContext as NutritionPlanMainViewModel;
            model.SelectedConsumed.CollectionChanged -= SelectedConsumed_CollectionChanged;
            var items = listConsumed.SelectedItems;
            model.SelectedConsumed.Clear();
            foreach (var i in items)
            {
                model.SelectedConsumed.Add(i as LebensmittelConsumedVM);
            }

            model.SelectedConsumed.CollectionChanged += SelectedConsumed_CollectionChanged;

        }

        void SelectedConsumed_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            model = DataContext as NutritionPlanMainViewModel;
            if (model.SelectedConsumed.Count == 0)
            {
                listConsumed.SelectedItems.Clear();
            }
        }
        

        private void LongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var userControlList = new List<UserControl>();
            GetItemsRecursive<UserControl>(sender as DependencyObject, ref userControlList);

            // Selected.
            if (e.AddedItems.Count > 0 && e.AddedItems[0] != null)
            {
                foreach (var userControl in userControlList)
                {
                    if (e.AddedItems[0].Equals(userControl.DataContext))
                    {
                        VisualStateManager.GoToState(userControl, "Selected", true);
                    }
                }
            }
            // Unselected.
            if (e.RemovedItems.Count > 0 && e.RemovedItems[0] != null)
            {
                foreach (var userControl in userControlList)
                {
                    if (e.RemovedItems[0].Equals(userControl.DataContext))
                    {
                        VisualStateManager.GoToState(userControl, "Normal", true);
                    }
                }
            }
        }


        /// <summary>
        /// Recursively get the item.
        /// </summary>
        /// <typeparam name="T">The item to get.</typeparam>
        /// <param name="parents">Parent container.</param>
        /// <param name="objectList">Item list</param>
        public static void GetItemsRecursive<T>(DependencyObject parents, ref List<T> objectList) where T : DependencyObject
        {
            var childrenCount = VisualTreeHelper.GetChildrenCount(parents);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parents, i);

                if (child is T)
                {
                    objectList.Add(child as T);
                }

                GetItemsRecursive<T>(child, ref objectList);
            }

            return;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void listLeb_JumpListClosed(object sender, EventArgs e)
        {
            var userControlList = new List<UserControl>();
            GetItemsRecursive<UserControl>(sender as DependencyObject, ref userControlList);

            foreach (var userControl in userControlList)
            {
                if (userControl.DataContext is LebensmittelVM)
                    VisualStateManager.GoToState(userControl, "Normal", true);
            }
            (DataContext as NutritionPlanMainViewModel).Lebensmittel=null;
        }

    }
}
