using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace BettrDiet.Core.ViewModels
{
    using BettrDiet.Core.Common;
    using BettrDiet.Core.Events;
    using BettrFitSPA.Viewmodels.User;
    using Cirrious.CrossCore;
    using Cirrious.MvvmCross.Plugins.Messenger;
    using Cirrious.MvvmCross.ViewModels;

    public class GoalEditViewModel : BaseViewModel
    {
        private BettrFitDataSource _ds = BettrFitDataSource.Instance;
        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _logintoken;
        private MvxSubscriptionToken _synctoken;

        private Dictionary<int, int> mapDays = new Dictionary<int, int> { { 0, 1 }, { 1, 3 }, { 2, 5 } };
        private Dictionary<int, int> mapMinutes = new Dictionary<int, int> { { 0, 30 }, { 1, 45 }, { 2, 60 } };

        public GoalEditViewModel()
        {
            //this.DefaultViewModel["Lebensmittel"] = ds.CurrentLebensmittel;
            //this.DefaultViewModel["Items"] = ds.LebensmittelConsumed;
            //this.DefaultViewModel["SelectedConsumed"] = ds.SelectedConsumed;
            //this.DefaultViewModel["Search"] = ds.LebensmittelResult;
            //this.DefaultViewModel["CurrentLebensmittel"] = ds.CurrentLebensmittel;
            //this.DefaultViewModel["SummaryConsumedDay"] = ds.SummaryConsumedDay;
            //this.DefaultViewModel["SummaryConsumedDaytime"] = ds.SummaryConsumedDaytime;

            //SelectedConsumed = null;

            _messenger = Mvx.Resolve<IMvxMessenger>();
            //_logintoken = _messenger.Subscribe<LoggedInEvent>(a => LoginChanged(a.LoggedIn));
            _synctoken = _messenger.Subscribe<SyncEvent>(a => RaisePropertyChanged("Sync"));
            SaveDailyCommand = new MvxCommand(SaveDaily, canSaveDaily);
            NewRecord = false;

            WorkoutMinutes = new ObservableCollection<int>() { 20, 40, 60 };
            WorkoutDays = new ObservableCollection<int>() { 1, 3, 5 };

            _goal_Endurance = CurrentGoal.Goal_Endurance;
            _goal_FatLoss = CurrentGoal.Goal_FatLoss;
            _goal_Muscle = CurrentGoal.Goal_Muscle;
            OnPropertyChanged("Goal_Muscle");
            OnPropertyChanged("Goal_Endurance");
            OnPropertyChanged("Goal_FatLoss"); ;
        }

        private bool canSaveDaily()
        {
            return true;
        }

        private void SaveDaily()
        {
            if (CurrentGoal._id == null)
                _ds.AddUserGoal(CurrentGoal);
            else
                _ds.UpdateUserGoal(CurrentGoal);

            this.Close(this);
        }

        private int _selectedDays = 0;

        public int SelectedDays
        {
            get
            {
                if (CurrentGoal == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return -1;
                }

                return mapDays.FirstOrDefault(a => a.Value == CurrentGoal.WorkoutDays).Key;
            }
            set
            {
                if (CurrentGoal == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return;
                }
                CurrentGoal.WorkoutDays = (short)mapDays[value];

                SetProperty(ref _selectedDays, value, () => SelectedDays);
            }
        }

        public int SelectedMinutes
        {
            get
            {
                if (CurrentGoal == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return -1;
                }

                return mapMinutes.FirstOrDefault(a => a.Value == CurrentGoal.WorkoutMinutes).Key;
            }
            set
            {
                if (CurrentGoal == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return;
                }
                CurrentGoal.WorkoutMinutes = (short)mapMinutes[value];

                SetProperty(ref _selectedDays, value, () => SelectedMinutes);
            }
        }

        private double _goal_Endurance;

        public double Goal_Endurance
        {
            get { return _goal_Endurance; }
            set
            {
                SetProperty(ref _goal_Endurance, value, () => Goal_Endurance);
                calcMax();
                if (CurrentGoal != null)
                    CurrentGoal.Goal_Endurance = (byte)_goal_Endurance;
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
                if (CurrentGoal != null)
                    CurrentGoal.Goal_Muscle = (byte)_goal_Muscle;
            }
        }

        private double _goal_FatLoss;

        public double Goal_FatLoss
        {
            get { return _goal_FatLoss; }
            set
            {
                SetProperty(ref _goal_FatLoss, value, () => Goal_FatLoss);
                calcMax();
                if (CurrentGoal != null)
                    CurrentGoal.Goal_FatLoss = (byte)_goal_FatLoss;
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

        public UserGoalVM CurrentGoal
        {
            get
            {
                if (_ds.CurrentGoal == null)    // Copy Data if empty
                {
                    _ds.CurrentGoal = new UserGoalVM();

                    var src = _ds.UserGoals.OrderByDescending(a => a.DestinationDate).FirstOrDefault();
                    if (src != null)
                    {
                        AutoMapper.Mapper.Map<UserGoalVM, UserGoalVM>(src, _ds.CurrentGoal);
                    }

                    _ds.CurrentGoal.Date = DateTime.Now;
                    _ds.CurrentGoal._id = null;
                }

                return _ds.CurrentGoal;
            }
            set { _ds.CurrentGoal = value; }
        }

        public bool Sync
        {
            get
            {
                Debug.WriteLine("Syncin:" + _ds._sync.IsSyncing);
                return _ds._sync.IsSyncing;
            }
        }

        #region Commands

        public MvxCommand SaveDailyCommand
        {
            get;
            private set;
        }

        public MvxCommand DeleteDailyCommand
        {
            get;
            private set;
        }

        public MvxCommand EditDailyCommand
        {
            get;
            private set;
        }

        #endregion Commands

        public bool NewRecord { get; set; }

        public ObservableCollection<int> WorkoutMinutes { get; set; }

        public ObservableCollection<int> WorkoutDays { get; set; }
    }
}