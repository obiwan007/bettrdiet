using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Cirrious.MvvmCross.ViewModels;
using BettrFitSPA.Viewmodels.User;
using BettrDiet.Core.Common;


namespace BettrDiet.Core.ViewModels
{
    public class UserGoalOverviewViewModel : BaseViewModel
    {

        BettrFitDataSource ds = BettrFitDataSource.Instance;

        public UserGoalOverviewViewModel()
        {
            //this.DefaultViewModel["Lebensmittel"] = ds.CurrentLebensmittel;
            //this.DefaultViewModel["Items"] = ds.LebensmittelConsumed;
            //this.DefaultViewModel["SelectedConsumed"] = ds.SelectedConsumed;
            //this.DefaultViewModel["Search"] = ds.LebensmittelResult;
            //this.DefaultViewModel["CurrentLebensmittel"] = ds.CurrentLebensmittel;
            //this.DefaultViewModel["SummaryConsumedDay"] = ds.SummaryConsumedDay;
            //this.DefaultViewModel["SummaryConsumedDaytime"] = ds.SummaryConsumedDaytime;            

            ds.FillUserGoals();
            //SelectedConsumed = null;


            AddGoalCommand = new MvxCommand(AddGoal, canAddGoal);
            DeleteGoalCommand = new MvxCommand(DeleteGoal, canDeleteGoal);
            EditGoalCommand = new MvxCommand(EditGoal, canEditGoal);
        }



        private bool canAddGoal()
        {
                return true;
        }

        private void AddGoal()
        {
            SelectedGoal = null;
            //ds.CurrentDailyData = null;
            //CurrentFrame.NavigationService.Navigate(new Uri("/Views/UserGoalEdit.xaml", UriKind.Relative));
            this.ShowViewModel<UserGoalEditViewModel>();
        }

        private bool canDeleteGoal()
        {
            if (SelectedGoal != null)
                return true;
            else
                return false;
        }

        private void DeleteGoal()
        {
            //ds.DeleteUserGoal(SelectedGoal);
        }

        private bool canEditGoal()
        {
            if (SelectedGoal != null)
                return true;
            else
                return false;
        }

        private void EditGoal()
        {
            this.ShowViewModel<UserGoalEditViewModel>();
            //CurrentFrame.NavigationService.Navigate(new Uri("/Views/UserGoalEdit.xaml", UriKind.Relative));
        }



        private UserGoalVM _selectedGoal;

        public UserGoalVM SelectedGoal
        {
            get { return _selectedGoal; }
            set {
                this.SetProperty(ref _selectedGoal, value,()=> SelectedGoal);
                
                DeleteGoalCommand.RaiseCanExecuteChanged();
                EditGoalCommand.RaiseCanExecuteChanged();
                ds.SelectedUserGoal = _selectedGoal;
            }
        }
        

        public ObservableCollection<UserGoalVM> Goals
        {
            get { return ds.UserGoals; }            
        }
        
      



        #region Commands

        public MvxCommand AddGoalCommand
        {
            get;
            private set;
        }

        public MvxCommand DeleteGoalCommand
        {
            get;
            private set;
        }

        public MvxCommand EditGoalCommand
        {
            get;
            private set;
        }

        #endregion Commands
        
        
        


    }
}
