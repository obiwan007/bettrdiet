using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Cirrious.MvvmCross.ViewModels;
using BettrFitSPA.Viewmodels;
using BettrFitSPA.Viewmodels.User;
using BettrDiet.Core.Common;


namespace BettrDiet.Core.ViewModels
{
    public class UserGoalEditViewModel : BaseViewModel
    {

        BettrFitDataSource ds = BettrFitDataSource.Instance;

        public ObservableCollection<int> WorkoutMinutes { get; set; }
        public ObservableCollection<int> WorkoutDays{ get; set; }

        Dictionary<int, int> mapDays = new Dictionary<int, int> { { 0, 1 }, { 1, 3 }, { 2, 5 } };
        Dictionary<int, int> mapMinutes = new Dictionary<int, int> { { 0, 30 }, { 1, 45 }, { 2, 60 } };


        public UserGoalEditViewModel()
        {
            //this.DefaultViewModel["Lebensmittel"] = ds.CurrentLebensmittel;
            //this.DefaultViewModel["Items"] = ds.LebensmittelConsumed;
            //this.DefaultViewModel["SelectedConsumed"] = ds.SelectedConsumed;
            //this.DefaultViewModel["Search"] = ds.LebensmittelResult;
            //this.DefaultViewModel["CurrentLebensmittel"] = ds.CurrentLebensmittel;
            //this.DefaultViewModel["SummaryConsumedDay"] = ds.SummaryConsumedDay;
            //this.DefaultViewModel["SummaryConsumedDaytime"] = ds.SummaryConsumedDaytime;            

            //ds.FillDailyData();
            //SelectedConsumed = null;

            WorkoutMinutes = new ObservableCollection<int>() { 20,40, 60 };
            WorkoutDays= new ObservableCollection<int>() { 1, 3, 5};

            SaveGoalCommand = new MvxCommand(SaveGoal, canSaveGoal);


            _goal_Endurance = SelectedUserGoal.Goal_Endurance;
            _goal_FatLoss = SelectedUserGoal.Goal_FatLoss;
            _goal_Muscle = SelectedUserGoal.Goal_Muscle;
            OnPropertyChanged("Goal_Muscle");
            OnPropertyChanged("Goal_Endurance");
            OnPropertyChanged("Goal_FatLoss"); ;

        }

        private bool canSaveGoal()
        {
                return true;
        }

        private void SaveGoal()
        {
            if(NewRecord)
                ds.AddUserGoal(SelectedUserGoal);
            else
                ds.UpdateUserGoal(SelectedUserGoal);
            //CurrentFrame.NavigationService.GoBack();
        }

        

        public UserGoalVM SelectedUserGoal
        {
            get {
                if (ds.SelectedUserGoal == null)    // Copy Data if empty
                {
                    ds.SelectedUserGoal = new UserGoalVM();

                    var src=ds.UserGoals.OrderByDescending(a => a.Date).FirstOrDefault();
                    if (src != null)
                    {
                        AutoMapper.Mapper.Map<UserGoalVM, UserGoalVM>(src, SelectedUserGoal);
                    }

                    SelectedUserGoal.Date = DateTime.Today;
                    SelectedUserGoal.DestinationDate = DateTime.Today;

                    SelectedUserGoal.Goal_Muscle = 8;
                    SelectedUserGoal.Goal_FatLoss = 5;
                    SelectedUserGoal.Goal_Endurance = 2;
                    SelectedUserGoal.WorkoutDays = 3;
                    SelectedUserGoal.WorkoutMinutes = 45;

                    NewRecord = true;
                }
                else
                    NewRecord=false;

                return ds.SelectedUserGoal;
            }
            set { ds.SelectedUserGoal = value; }
        }


        private int _selectedDays = 0;
        public int SelectedDays
        {
            get
            {
                if (SelectedUserGoal == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return -1;
                }

                return mapDays.FirstOrDefault(a => a.Value == SelectedUserGoal.WorkoutDays).Key;


            }
            set
            {
                if (SelectedUserGoal == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return;
                }
                SelectedUserGoal.WorkoutDays = (short)mapDays[value];

                SetProperty(ref _selectedDays, value, ()=>SelectedDays);
            }
        }

        public int SelectedMinutes
        {
            get
            {
                if (SelectedUserGoal == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return -1;
                }

                return mapMinutes.FirstOrDefault(a => a.Value == SelectedUserGoal.WorkoutMinutes).Key;


            }
            set
            {
                if (SelectedUserGoal == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return;
                }
                SelectedUserGoal.WorkoutMinutes = (short)mapMinutes[value];

                SetProperty(ref _selectedDays, value, ()=>SelectedMinutes);
            }
        }

        private double _goal_Endurance;
        public double Goal_Endurance
        {
            get { return _goal_Endurance; }
            set
            {
                SetProperty(ref _goal_Endurance, value, ()=>Goal_Endurance);
                calcMax();
                if (SelectedUserGoal != null)
                    SelectedUserGoal.Goal_Endurance = (byte)_goal_Endurance;
            }
        }
        private double _goal_Muscle;
        public double Goal_Muscle
        {
            get { return _goal_Muscle; }
            set
            {
                SetProperty(ref _goal_Muscle, value, () => Goal_Muscle);
                calcMax();
                if (SelectedUserGoal != null)
                    SelectedUserGoal.Goal_Muscle = (byte)_goal_Muscle;
            }
        }
        private double _goal_FatLoss;
        public double Goal_FatLoss
        {
            get { return _goal_FatLoss; }
            set
            {
                SetProperty(ref _goal_FatLoss, value, ()=>Goal_FatLoss);
                calcMax();
                if (SelectedUserGoal != null)
                    SelectedUserGoal.Goal_FatLoss = (byte)_goal_FatLoss;

            }
        }

        private void calcMax()
        {
            var sum = Goal_Endurance + Goal_Muscle + Goal_FatLoss;
            int dif = (int)(15 - sum);
            if (dif >= 0)
                return;
            if (Goal_Endurance > 1)
                Goal_Endurance -= 1;


            if (Goal_FatLoss > 1)
                Goal_FatLoss -= 1;


            if (Goal_Muscle > 0)
                Goal_Muscle -= 1;
        }



        #region Commands

        public MvxCommand SaveGoalCommand
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
        
       
        public bool NewRecord { get; set; }
    }
}
