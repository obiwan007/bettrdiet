// --------------------------------------------------------------------------------------------------------------------
// <summary>
//    Defines the FirstViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace BettrDiet.Core.ViewModels
{
    using System.Windows.Input;
    using Cirrious.MvvmCross.ViewModels;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using BettrFit.Core.Common;
    using BettrFitSPA.Viewmodels.User;
    using System;
    using Cirrious.CrossCore;
    using System.Linq;
    using Cirrious.MvvmCross.Plugins.Messenger;
    using BettrDiet.Core.Events;
    using BettrDiet.Core.Common;
    using System.ServiceModel;
    using System.Diagnostics;
    using BettrFitSPA.Viewmodels;


    /// <summary>
    /// Define the FirstViewModel type.
    /// </summary>
    public class NutritionPlanOverviewViewModel : BaseViewModel
    {
        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _synctoken;
        private BettrFitDataSource _ds;

        public NutritionPlanOverviewViewModel()
        {
            _ds = BettrFitDataSource.Instance;

            _messenger = Mvx.Resolve<IMvxMessenger>();
            _synctoken = _messenger.Subscribe<SyncEvent>(a => RaisePropertyChanged("Sync"));
            CurrentDate = DateTime.Now;

            NutritionPlanCreateCommand = new MvxCommand(OnCreateNutritionPlan);

        }

        private void OnCreateNutritionPlan()
        {
            this.ShowViewModel(typeof(NutritionPlanCreateViewModel));
        }
       
        public bool Sync
        {
            get
            {
                Debug.WriteLine("Syncin:" + _ds._sync.IsSyncing);
                return _ds._sync.IsSyncing;
            }
        }

        public NutritionPlanVM NutritionPlan
        {
            get { return _ds.CurrentNutritionPlan; }
        }

        public NutritionPlanDayVM NutritionPlanDay
        {
            get
            {

                var d = (CurrentDate - NutritionPlan.StartDate).Days;

                if (d > -1 && d < NutritionPlan.Days.Count)
                    return NutritionPlan.Days[d];
                else
                    return null;
            }
        }

        public int NutritionPlanDayIndex
        {
            get
            {
                var d = (CurrentDate - NutritionPlan.StartDate).Days;
                return d;
            }
        }

        public DateTime CurrentDate { get; set; }

        public MvxCommand NutritionPlanCreateCommand { get; set; }
    }
}
