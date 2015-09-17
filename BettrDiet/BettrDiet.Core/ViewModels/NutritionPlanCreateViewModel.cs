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
    public class NutritionPlanCreateViewModel : BaseViewModel
    {
        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _synctoken;
        private BettrFitDataSource _ds;

        public NutritionPlanCreateViewModel()
        {
            _ds = BettrFitDataSource.Instance;

            _messenger = Mvx.Resolve<IMvxMessenger>();
            _synctoken = _messenger.Subscribe<SyncEvent>(a => RaisePropertyChanged("Sync"));
            CurrentDate = DateTime.Now;

            NutritionPlanCreateCommand = new MvxCommand(OnCreateNutritionPlan);

            SelectedDate = DateTime.Now;
            
            PlanList = new ObservableCollection<string>();

            AllPlans = new ObservableCollection<NutritionPlanVM>();

            fillPlanlist();

            _ds.CalcBMR(_ds.UserData, ref _bmr, ref _act, ref _weight,ref _calcString);
            SelectedCalories = (int)(ACT*0.9d);
        }

        private bool _canCreatePlan=false;
        public bool canCreatePlan
        {
            get { return _canCreatePlan; }
            set { SetProperty(ref _canCreatePlan, value, () => canCreatePlan);
            }
        }
        

        private async void OnCreateNutritionPlan()
        {
            _ds._sync.BeginSync();
            canCreatePlan = false;

            var p = await WebClientAsyncExtensions.GetNewNutritionPlanTask(_ds.getAuth(),NutritionPlan.PlanType,SelectedCalories,CurrentDate);
            _ds.CurrentNutritionPlan = AutoMapper.Mapper.Map<WebAccess.ServiceReference.NutritionPlanVM, NutritionPlanVM>(p);


            await _ds.FillNutritionPlanFavorites(NutritionPlan.PlanType);
            await _ds.FillNutritionPlanLebensmittel(NutritionPlan.PlanType);
            _ds.SaveAll();
            _ds._sync.EndSync();
            this.Close(this);
        }
       
        public bool Sync
        {
            get
            {
                Debug.WriteLine("Syncin:" + _ds._sync.IsSyncing);
                return _ds._sync.IsSyncing;
            }
        }

        private string _calcString;

        public string CalcString
        {
            get { return _calcString; }
            set { _calcString = value; }
        }
        

        private double _weight;
        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }


        private double _bmr;
        public double BMR
        {
            get { return _bmr; }
            set { _bmr = value; }
        }

        private double _act;

        public double ACT
        {
            get { return _act; }
            set { _act = value; }
        }

        private DateTime _date;
        public DateTime SelectedDate
        {
            get { return _date; }
            set { _date = value; }
        }

        public int SelectedCalories { get; set; }

        public ObservableCollection<string> PlanList { get; set; }


        private NutritionPlanVM _nutritionPlan;
        public NutritionPlanVM NutritionPlan
        {
            get { return _nutritionPlan; }
            set {
                SetProperty(ref _nutritionPlan, value, () => NutritionPlan);
                PlannedDays = NutritionPlan.Days.Count;

                RaisePropertyChanged("PlannedDays");
                RaiseAllPropertiesChanged();
            }
        }

        public int PlannedDays { get; set; }

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

       
        private int _selectedPlanIndex=-1;
        public int SelectedPlanIndex
        {
            get { return _selectedPlanIndex; }
            set {
                SetProperty(ref _selectedPlanIndex, value, () => SelectedPlanIndex);

                if (value>-1 && value<AllPlans.Count)
                    NutritionPlan = AllPlans[value];
            }
        }
        

        public DateTime CurrentDate { get; set; }

        public MvxCommand NutritionPlanCreateCommand { get; set; }

        private async void fillPlanlist()
        {
            _ds._sync.BeginSync();
            PlanList.Clear();
            var pl=await WebClientAsyncExtensions.GetNutritionPlans(_ds.getAuth());
            foreach(var p in pl)
            {
                PlanList.Add(p.Name);
                AllPlans.Add(AutoMapper.Mapper.Map<WebAccess.ServiceReference.NutritionPlanVM, NutritionPlanVM>(p));
            }
            RaisePropertyChanged("PlanList");
            SelectedPlanIndex = 0;
            canCreatePlan = true;
            _ds._sync.EndSync();
        }


        public ObservableCollection<NutritionPlanVM> AllPlans { get; set; }
    }
}
