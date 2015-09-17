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
    using System.Threading.Tasks;
    using BettrFitSPA.Viewmodels;


    public class DataEntry : BaseViewModel
    {
        public DataEntry(string cat, double val)
        {
            Cat = cat;
            Val = val;
        }
        public string Cat { get; set; }
        private double _val;

        public double Val
        {
            get { return _val; }
            set { SetProperty(ref _val, value, ()=>Val); }
        }
        
    }

    /// <summary>
    /// Define the FirstViewModel type.
    /// </summary>
    public class NutritionPlanMainViewModel : BaseViewModel
    {
        BettrFitDataSource _ds;

        string menuNutritionPlan = CultureHelper.GetLocalString("Your Nutrition Plan|Dein Ernährungsplan");
        string menuNutritiondiary = CultureHelper.GetLocalString("Nutrition Diary|Ernährungstagebuch");
        string menuGoals = CultureHelper.GetLocalString("Goals|Ziele");
        string menuNutrition = CultureHelper.GetLocalString("Nutrition|Nahrungsmittel");
        string menuWeight = CultureHelper.GetLocalString("Weight Diary|Gewichtstagebuch");
        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _synctoken;
        private Plan1PointsVM _remainingPlanPoints = new Plan1PointsVM();
        private Plan1PointsVM _diffPlanPoints = new Plan1PointsVM();
        private bool _isRefreshing = false;


        public NutritionPlanMainViewModel()
        {
            _ds = BettrFitDataSource.Instance;

            Verbraucht = new ObservableCollection<DataEntry>();
            Verbraucht.Add(new DataEntry(CultureHelper.GetLocalString("Carbs|KH"), 0));
            Verbraucht.Add(new DataEntry(CultureHelper.GetLocalString("Protein|Eiweiß"), 0));
            Verbraucht.Add(new DataEntry(CultureHelper.GetLocalString("Fats|Fette"), 0));
            Verbraucht.Add(new DataEntry(CultureHelper.GetLocalString("Energie"), 0));

            Uebrig = new ObservableCollection<DataEntry>();
            Uebrig.Add(new DataEntry(CultureHelper.GetLocalString("Carbs|KH"), 0));
            Uebrig.Add(new DataEntry(CultureHelper.GetLocalString("Proteins|Eiweiß"), 0));
            Uebrig.Add(new DataEntry(CultureHelper.GetLocalString("Fats|Fette"), 0));
            Uebrig.Add(new DataEntry(CultureHelper.GetLocalString("Energie"), 0));


            _messenger = Mvx.Resolve<IMvxMessenger>();
            _synctoken = _messenger.Subscribe<SyncEvent>(a => RaisePropertyChanged("Sync"));
            _nonetworktoken = _messenger.SubscribeOnMainThread<NetworkEvent>(m =>
                { 
                    if(m.IsAvailable==false)
                    {
                        this.Close(this);
                    }
                });

            RefreshCommand = new MvxCommand(OnRefreshCommand);
            AddLebensmittelCommand = new MvxCommand(AddLebensmittel);
            DeleteConsumedCommand = new MvxCommand(DeleteConsumed, () => canDeleteConsumed);
            CopyConsumedCommand = new MvxCommand(CopyConsumed, () => canCopyConsumed);
            CopyDayConsumedCommand = new MvxCommand(CopyDayConsumed, () => canCopyDayConsumed);
            PasteConsumedCommand = new MvxCommand(PasteConsumed, () => canPasteConsumed);
            SelectFavoriteCommand = new MvxCommand(OnSelectFavoriteConsumed);
            ShowFilterCommand = new MvxCommand(OnShowFilter);
            FilterCloseCommand = new MvxCommand(OnFilterClose);
            FeedbackCommand = new MvxCommand(OnFeedback);
            BackCommand = new MvxCommand(() => { this.Close(this); });
            SelectedConsumed.CollectionChanged += SelectedConsumed_CollectionChanged;

            DayTimes = new List<string>() { "Breakfast|Frühstück", "2. Breakfast|2. Frühstück", "Lunch|Mittag", "Snack 1|Zwischen 1", "Pre Workout", "Post Workout", "Dinner|Abendessen", "Snack 2|Zwischen 2" };
            DayTimes=DayTimes.Select(a => CultureHelper.GetLocalString(a)).ToList();
            FirstInit();
        }

        private void OnFeedback()
        {
            this.ShowViewModel(typeof(FeedbackViewModel));
        }

        private void OnFilterClose()
        {
            IsShowFilterOpen = false;
            RaisePropertyChanged("LebensmittelListe");
        }

        private void OnShowFilter()
        {
            IsShowFilterOpen = true;
        }

        public async void FirstInit()
        {
            Consumed.Clear();

            if (NutritionPlanDay==null)
            {
                _messenger.Publish<PopupEvent>(new PopupEvent(this, 
                    
                    CultureHelper.GetLocalString("Your current nutrition plan is finished. Please create a new one.|Dein aktueller Ernährungsplan ist beendet.\r\nBitte erstelle einen neuen."), "Info"));
                return;
            }

            try
            {
                await _ds.SearchConsumed(CurrentDayTime, CurrentDate, Consumed);
                UpdatePlan1Points();
            }
            catch(Exception ex)
            {
                this.Close(this);
            }
        }


        private async void OnRefreshCommand()
        {
            IsNotRefreshing = false;
            await _ds.RefreshData();
            IsNotRefreshing = true;
        }

        private bool _isNotRefreshing = true;
        public bool IsNotRefreshing
        {
            get { return _isNotRefreshing; }
            set { SetProperty(ref _isNotRefreshing, value, () => IsNotRefreshing); }
        }

        private void OnSelectFavoriteConsumed()
        {
            SelectFavorite = true;
            RaisePropertyChanged("SelectFavorite");
        }

        public List<string> Favorites
        {
            get
            {
                var l = _ds.NutritionPlanFavorites.Select(a => a.Name).ToList();
                l.Insert(0, "");
                return l;
            }
        }

        public short CurrentFavorite
        {
            get { return 0; }
            set
            {
                var fav = value;
                if (fav > 0)
                {
                    AddFavorites(fav);
                }
            }
        }

        private async Task AddFavorites(short fav)
        {
            _ds._sync.BeginSync();
            var d = _ds.NutritionPlanFavorites[fav - 1];

            foreach (var lc in d.Lebensmittel)
            {
                await WebClientAsyncExtensions.AddConsumedTask(_ds.getAuth(), lc.Lebensmittel_id, lc.Menge, CurrentDayTime, CurrentDate);
            }
            _ds.SearchConsumed(CurrentDayTime, CurrentDate, Consumed);
            UpdatePlan1Points();

            RaisePropertyChanged("CurrentFavorite");
            RaisePropertyChanged("SelectFavorite");
            _ds._sync.EndSync();
        }

        public bool Sync
        {
            get
            {
                Debug.WriteLine("Syncin:" + _ds._sync.IsSyncing);
                return _ds._sync.IsSyncing;
            }
        }

        public List<string> DayTimes { get; set; }

        public bool SelectFavorite { get; set; }

        public NutritionPlanVM NutritionPlan
        {
            get { return _ds.CurrentNutritionPlan; }
        }

        public NutritionPlanDayVM NutritionPlanDay
        {
            get
            {
                if (NutritionPlan == null)
                    return null;

                var d = (CurrentDate - NutritionPlan.StartDate.Date).Days;

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
                var d = (CurrentDate - NutritionPlan.StartDate.Date).Days;
                return d;
            }
        }


        void SelectedConsumed_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged("SelectedConsumedSum");
            RaisePropertyChanged("canDeleteConsumed");
            RaisePropertyChanged("canCopyConsumed");
            DeleteConsumedCommand.RaiseCanExecuteChanged();
            CopyConsumedCommand.RaiseCanExecuteChanged();
        }


        private int _dateIndex = 7;

        public int DateIndex
        {
            get { return _dateIndex; }
            set
            {
                _dateIndex = value;

                CurrentDate = DateTime.Today - TimeSpan.FromDays(7 - value);
            }
        }


        private short _currentDayTime = 0;
        public short CurrentDayTime
        {
            get { return _currentDayTime; }
            set
            {
                _currentDayTime = value;
                Consumed.Clear();
                SelectedConsumed.Clear();
                _ds.SearchConsumed(CurrentDayTime, CurrentDate, Consumed);

            }
        }


        public DateTime MinDate
        {
            get { return NutritionPlan.StartDate; }            
        }
        public DateTime MaxDate
        {
            get { return NutritionPlan.StartDate+TimeSpan.FromDays(NutritionPlan.Days.Count); }
        }
        

        private DateTime _currentDate = DateTime.Today;
        public DateTime CurrentDate
        {
            get { return _currentDate; }
            set
            {
                if(SetProperty(ref _currentDate, value,()=>CurrentDate))
                    OnUpdateDate();
            }
        }

        public async void OnUpdateDate()
        {
            Consumed.Clear();
            SelectedConsumed.Clear();
            await _ds.SearchConsumed(CurrentDayTime, CurrentDate, Consumed);
            RaisePropertyChanged("NutritionPlanDay");
            RaisePropertyChanged("NutritionPlanDayIndex");
            UpdatePlan1Points();
        }

        private ObservableCollection<LebensmittelConsumedVM> _consumed = new ObservableCollection<LebensmittelConsumedVM>();
        public ObservableCollection<LebensmittelConsumedVM> Consumed
        {
            get { return _consumed; }
            set { _consumed = value; }
        }

        private float _lebensmittelMenge;
        public float LebensmittelMenge
        {
            get { return _lebensmittelMenge; }
            set
            {
                SetProperty(ref _lebensmittelMenge, value, () => LebensmittelMenge);
            }
        }

        public ObservableCollection<DataEntry> Verbraucht { get; set; }
        
        public ObservableCollection<Double> Macros
        {
            get { return _ds.Macros; }
        }


        public Double SumFatP
        {
            get { return SummaryConsumedDay.SumFatP; }
        }


        private WebAccess.ServiceReference.SummaryData _summaryConsumedDay;
        public WebAccess.ServiceReference.SummaryData SummaryConsumedDay
        {
            get { return _ds.SummaryConsumedDay; }
            set { _summaryConsumedDay = value; }
        }

        private WebAccess.ServiceReference.SummaryData _summaryConsumedDaytime;
        public WebAccess.ServiceReference.SummaryData SummaryConsumedDaytime
        {
            get { return _ds.SummaryConsumedDaytime; }
            set { _summaryConsumedDaytime = value; }
        }


        public IEnumerable<Group<LebensmittelVM>> LebensmittelListe
        {
            get
            {
                //var g=ds.Exercises.GroupBy(a => a.Key);
                //var cityByCountry = from city in ds.Exercises
                //                    group city by city.Key into c
                //                    orderby c.Key
                //                    select new Group<ExerciseVM>(c.Key, c);

                //var g = ds.Exercises.Select(a=>new Group<ExerciseVM>(a.Key,a));
                var _selectedFilter = 1;

                //if (_search != null && _search.Length > 1)
                //{
                //    if (_selectedFilter == 0)
                //    {
                //        var gg = GetItemGroups<LebensmittelVM>(_ds.NutritionPlanLeb
                //        .Where(a => a.Name.IndexOf(_search, 0, StringComparison.CurrentCultureIgnoreCase) > -1), c => c.Name.Substring(0, 1));
                //        return gg;
                //    }
                //    else
                //    {
                //        var gg = GetItemGroups<LebensmittelVM>(_ds.NutritionPlanLeb
                //        .Where(a => a.Name.IndexOf(_search, 0, StringComparison.CurrentCultureIgnoreCase) > -1), c => c.Name.Substring(0, 1));
                //        return gg;
                //    }
                //}
                //else
                {
                    //var l = _ds.NutritionPlanLeb;
                    var l = _ds.NutritionPlanLeb.Where(m =>
                        ((m.IsGlutenFree == IsGlutenfrei && IsGlutenfrei == true)
                            || (m.IsVegan == IsVegan && IsVegan == true)
                            || (m.IsVegetarisch == IsVegetarisch && IsVegetarisch == true)
                            || (m.IsLactoseFree == IsLaktosefrei && IsLaktosefrei == true)
                        )
                        ||
                        (
                            IsLaktosefrei == false && IsVegan == false && IsGlutenfrei == false && IsVegetarisch == false
                        )
                        );
                    if (_selectedFilter == 0)
                    {
                        var l2=GetItemGroups<LebensmittelVM>(l, c => c.Name.Substring(0, 1));
                        return l2;
                    }
                    else
                    {
                        var l2=GetItemGroups<LebensmittelVM>(l, c => c.CurrentPlanType);
                        return l2;
                    }
                }
            }
        }

        public IEnumerable<Group<LebensmittelVM>> LebensmittelListe3
        {
            get
            {
                //var g=ds.Exercises.GroupBy(a => a.Key);
                //var cityByCountry = from city in ds.Exercises
                //                    group city by city.Key into c
                //                    orderby c.Key
                //                    select new Group<ExerciseVM>(c.Key, c);

                //var g = ds.Exercises.Select(a=>new Group<ExerciseVM>(a.Key,a));
                var _selectedFilter = 1;

                //if (_search != null && _search.Length > 1)
                //{
                //    if (_selectedFilter == 0)
                //    {
                //        var gg = GetItemGroups<LebensmittelVM>(_ds.NutritionPlanLeb
                //        .Where(a => a.Name.IndexOf(_search, 0, StringComparison.CurrentCultureIgnoreCase) > -1), c => c.Name.Substring(0, 1));
                //        return gg;
                //    }
                //    else
                //    {
                //        var gg = GetItemGroups<LebensmittelVM>(_ds.NutritionPlanLeb
                //        .Where(a => a.Name.IndexOf(_search, 0, StringComparison.CurrentCultureIgnoreCase) > -1), c => c.Name.Substring(0, 1));
                //        return gg;
                //    }
                //}
                //else
                {
                    //var l = _ds.NutritionPlanLeb;
                    var l = _ds.NutritionPlanLeb.Where(m =>
                        ((m.IsGlutenFree == IsGlutenfrei && IsGlutenfrei == true)
                            || (m.IsVegan == IsVegan && IsVegan == true)
                            || (m.IsVegetarisch == IsVegetarisch && IsVegetarisch == true)
                            || (m.IsLactoseFree == IsLaktosefrei && IsLaktosefrei == true)
                        )
                        ||
                        (
                            IsLaktosefrei == false && IsVegan == false && IsGlutenfrei == false && IsVegetarisch == false
                        )
                        );
                    if (_selectedFilter == 0)
                    {
                        return GetItemGroups<LebensmittelVM>(l, c => c.Name.Substring(0, 1));
                    }
                    else
                    {
                        return GetItemGroups<LebensmittelVM>(l, c => c.CurrentPlanType);
                    }
                }
            }
        }

        private ObservableCollection<LebensmittelVM> _searchResult = new ObservableCollection<LebensmittelVM>();
        public ObservableCollection<LebensmittelVM> LebensmittelListe2
        {
            get { return _ds.NutritionPlanLeb; }
        }


        public LebensmittelConsumedVM SelectedConsumedSum
        {
            get
            {
                if (SelectedConsumed.Count > 0)
                    return SelectedConsumed[0] as LebensmittelConsumedVM;
                else
                    return null;
            }
        }

        private ObservableCollection<LebensmittelConsumedVM> _selectedConsumed = new ObservableCollection<LebensmittelConsumedVM>();
        public ObservableCollection<LebensmittelConsumedVM> SelectedConsumed
        {
            get { return _selectedConsumed; }
            set
            {
                _selectedConsumed = value;
                RaisePropertyChanged("canDeleteConsumed");
                RaisePropertyChanged("canCopyConsumed");
                DeleteConsumedCommand.RaiseCanExecuteChanged();
            }
        }

        public Plan1PointsVM RemainingPlanPoints
        {
            get
            {
                return _remainingPlanPoints;
            }
        }

        public Plan1PointsVM DiffPlanPoints
        {
            get
            {
                return _diffPlanPoints;
            }
        }

        public DayTimesVM RemainingPlanPointsDaytime
        {
            get
            {
                if (_remainingPlanPoints != null && CurrentDayTime < _remainingPlanPoints.Daytimes.Count)
                    return _remainingPlanPoints.Daytimes[CurrentDayTime];
                else
                    return null;
            }
        }

        private async void UpdatePlan1Points()
        {
            var ret = await _ds.GetRemainingPlan1Points(CurrentDate);
            if (ret==null)
            {
                return;
            }
            AutoMapper.Mapper.Map<Plan1PointsVM, Plan1PointsVM>(ret, _remainingPlanPoints);

            _messenger.Publish<KalorienUpdatedEvent>(new KalorienUpdatedEvent(this, SummaryConsumedDay.SumkCal + " kCal / " + ret.SummeKCal.ToString()));

            _diffPlanPoints.KCal_Fette = _remainingPlanPoints.KCal_Fette - (float)SummaryConsumedDay.SumFat*9;
            _diffPlanPoints.KCal_Proteine = _remainingPlanPoints.KCal_Proteine - (float)SummaryConsumedDay.SumProt * 4;
            _diffPlanPoints.KCal_Kohlenhydrate = _remainingPlanPoints.KCal_Kohlenhydrate - (float)SummaryConsumedDay.SumKH * 4;
            _diffPlanPoints.SummeKCal= _remainingPlanPoints.SummeKCal - (float)SummaryConsumedDay.SumkCal;

            if (_remainingPlanPoints.KCal_Kohlenhydrate>0)
                Verbraucht[0].Val= (SummaryConsumedDay.SumKH * 4)/_remainingPlanPoints.KCal_Kohlenhydrate*100;

            if (_remainingPlanPoints.KCal_Proteine > 0) 
                Verbraucht[1].Val = (SummaryConsumedDay.SumProt * 4) / _remainingPlanPoints.KCal_Proteine * 100;
            if (_remainingPlanPoints.KCal_Fette > 0)
                Verbraucht[2].Val = (SummaryConsumedDay.SumFat * 9) / _remainingPlanPoints.KCal_Fette * 100;
            Verbraucht[3].Val = SummaryConsumedDay.SumkCal/_remainingPlanPoints.SummeKCal*100;

            for (int i = 0; i < 4;i++ )
                Uebrig[i].Val = 100 - Verbraucht[i].Val;

            RaisePropertyChanged("RemainingPlanPoints");
            RaisePropertyChanged("DiffPlanPoints");
            RaisePropertyChanged("Verbraucht");
            RaisePropertyChanged("RemainingPlanPointsDaytime");
        }

        public LebensmittelVM Lebensmittel
        {
            get { return _ds.CurrentLebensmittel; }
            set
            {
                _ds.CurrentLebensmittel = value;
                AddLebensmittelCommand.RaiseCanExecuteChanged();
                if (value != null)
                {
                    var m = _ds.CurrentLebensmittel.PlanEntries.FirstOrDefault(a => a.Plan == _ds.CurrentNutritionPlan.PlanType);
                    if (m != null)
                        LebensmittelMenge = float.Parse(m.MengeForEinheit);
                }
                RaisePropertyChanged("canAddLebensmittel");
                RaisePropertyChanged("Lebensmittel");
            }
        }

        public List<LebensmittelConsumedVM> CopyBuffer { get; set; }

        public bool copyDay { get; set; }


        #region Commands




        public bool canDeleteConsumed
        {
            get
            {
                if (SelectedConsumed.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        private async void DeleteConsumed()
        {
            foreach (var i in SelectedConsumed)
            {

                try
                {
                    var item = i as LebensmittelConsumedVM;
                    //ds.LebensmittelConsumed.Remove(i as LebensmittelConsumedViewModel);

                    await WebClientAsyncExtensions.DeleteConsumedTask(_ds.getAuth(), item._id);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in Deleting:" + ex);
                }
            }
            SelectedConsumed.Clear();
            Consumed.Clear();
            await _ds.SearchConsumed(CurrentDayTime, CurrentDate, Consumed);
            UpdatePlan1Points();
        }

        public bool canAddLebensmittel
        {
            get
            {
                if (Lebensmittel != null)
                    return true;
                else
                    return false;
            }
        }

        private async void AddLebensmittel()
        {
            var l = new LebensmittelConsumedVM();
            l.Lebensmittel_id = Lebensmittel._id;
            l.Name = Lebensmittel.Name;
            l.Mahlzeit = (short)CurrentDayTime;
            l.Menge = LebensmittelMenge;
            //ds.LebensmittelConsumed.Add(l);

            //SelectedConsumed = l;

            //Lebensmittel = null;            

            IsAddLebensmittelVisible = false;

            await WebClientAsyncExtensions.AddConsumedTask(_ds.getAuth(), l.Lebensmittel_id, LebensmittelMenge, CurrentDayTime, CurrentDate);
            //ds.LebensmittelConsumed.Clear();
            await _ds.SearchConsumed(CurrentDayTime, CurrentDate, Consumed);
            UpdatePlan1Points();

            IsAddLebensmittelVisible = true;
        }




        public bool canPasteConsumed
        {
            get
            {
                if (CopyBuffer != null && CopyBuffer.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        private async void PasteConsumed()
        {
            if (CopyBuffer == null)
                return;

            foreach (var i in CopyBuffer)
            {
                //AddConsumed(i.Menge, i.Lebensmittel);

                //var l = getConsumedFromLebensmittel(i.Menge, i.Lebensmittel);
                int daytime = i.Mahlzeit;
                if (copyDay == false)
                    daytime = CurrentDayTime;

                //await Task.Factory.FromAsync(
                //    WebService.Instance.WS.AddConsumedAsync(ds.getAuth(), i.Lebensmittel.ID, i.Menge, daytime, CurrentDate),
                //    WebService.Instance.WS.AddConsumedCompleted);

                await WebClientAsyncExtensions.AddConsumedTask(_ds.getAuth(), i.Lebensmittel_id, i.Menge, daytime, CurrentDate);
            }
            //ds.LebensmittelConsumed.Clear();
            Consumed.Clear();
            _ds.SearchConsumed(CurrentDayTime, CurrentDate, Consumed);
        }

        public bool canCopyDayConsumed
        {
            get
            {
                return true;
            }
        }

        private async void CopyDayConsumed()
        {
            CopyBuffer = new List<LebensmittelConsumedVM>();

            //ds.LebensmittelConsumed.Clear();
            for (int i = 0; i < 10; i++)
            {
                await getConsumed(i, CurrentDate, CopyBuffer);
            }
            PasteConsumedCommand.RaiseCanExecuteChanged();
            copyDay = true;
        }

        public bool canCopyConsumed
        {
            get
            {
                if (SelectedConsumed.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        private void CopyConsumed()
        {
            CopyBuffer = new List<LebensmittelConsumedVM>();
            foreach (var i in SelectedConsumed)
            {
                CopyBuffer.Add(i as LebensmittelConsumedVM);
            }
            copyDay = false;
            RaisePropertyChanged("canPasteConsumed");
        }

        public MvxCommand DeleteConsumedCommand
        {
            get;
            private set;
        }
        public MvxCommand CopyConsumedCommand
        {
            get;
            private set;
        }
        public MvxCommand CopyDayConsumedCommand
        {
            get;
            private set;
        }
        public MvxCommand PasteConsumedCommand
        {
            get;
            private set;
        }
        public MvxCommand SearchLebensmittelCommand
        {
            get;
            private set;
        }
        public MvxCommand AddLebensmittelCommand
        {
            get;
            private set;
        }
        public MvxCommand EditLebensmittelCommand
        {
            get;
            private set;
        }
        public MvxCommand NewLebensmittelCommand
        {
            get;
            private set;
        }

        public MvxCommand SelectFavoriteCommand { get; set; }

        public MvxCommand RefreshCommand { get; set; }

        #endregion Commands


        #region Helper
        public async Task getConsumed(int daytime, DateTime date, List<LebensmittelConsumedVM> col)
        {

            try
            {
                var result = await WebClientAsyncExtensions.GetConsumedTask(BettrFitDataSource.Instance.getAuth(), daytime, date);
                //var result = await WebService.Instance.WS.GetConsumedAsync(BettrFitDataSource.Instance.getAuth(), daytime, date);

                foreach (var i in result)
                {
                    var m = AutoMapper.Mapper.Map<WebAccess.ServiceReference.LebensmittelConsumedVM, LebensmittelConsumedVM>(i);
                    col.Add(m);
                }
            }
            catch(Exception ex)
            {

            }
        }

        #endregion






        public MvxCommand<string> NavCommand { get; set; }

        public ObservableCollection<string> NavCommands { get; set; }


        private static List<Group<T>> GetItemGroups<T>(IEnumerable<T> itemList, Func<T, string> getKeyFunc)
        {
            var d = new Dictionary<string, string>();
            d.Add("Gewuerze", "Spice|Gewürze");
            d.Add("Fette", "Fat|Fette");
            d.Add("Gemuese", "Veggetables|Gemüse");
            d.Add("Kohlenhydrate", "Carbs|Kohlenhydrate");
            d.Add("Milchprodukte", "Diary|Milchprodukte");
            d.Add("Obst", "Fruits|Obst");
            d.Add("Proteine", "Proteins|Eiweiß");
            d.Add("Snacks", "Snacks|Zwischenmahlzeit");


            IEnumerable<Group<T>> groupList = from item in itemList
                                              group item by getKeyFunc(item) into g
                                              orderby g.Key
                                              select new Group<T>(CultureHelper.GetLocalString(d[g.Key]), g);

            return groupList.ToList();
        }

        private bool _isAddLebensmittelVisible = true;
        public bool IsAddLebensmittelVisible
        {
            get { return _isAddLebensmittelVisible; }
            set { SetProperty(ref  _isAddLebensmittelVisible, value, () => IsAddLebensmittelVisible); }
        }



        public MvxCommand ShowFilterCommand { get; set; }

        private bool _isShowFilter;
        public bool IsShowFilterOpen
        {
            get { return _isShowFilter; }
            set { SetProperty(ref _isShowFilter, value, () => IsShowFilterOpen); }
        }

        private bool _isVegan;
        public bool IsVegan
        {
            get { return _isVegan; }
            set { SetProperty(ref _isVegan, value, () => IsVegan); }
        }
        private bool _isVegetarisch;
        public bool IsVegetarisch
        {
            get { return _isVegetarisch; }
            set { SetProperty(ref _isVegetarisch, value, () => IsVegetarisch); }
        }
        private bool _isGlutenfrei;
        public bool IsGlutenfrei
        {
            get { return _isGlutenfrei; }
            set { SetProperty(ref _isGlutenfrei, value, () => IsGlutenfrei); }
        }
        private bool _isLaktosefrei;
        public bool IsLaktosefrei
        {
            get { return _isLaktosefrei; }
            set { SetProperty(ref _isLaktosefrei, value, () => IsLaktosefrei); }
        }

        public MvxCommand FilterCloseCommand { get; set; }

        public MvxSubscriptionToken _nonetworktoken { get; set; }

        public ObservableCollection<DataEntry> Uebrig { get; set; }

        public MvxCommand FeedbackCommand { get; set; }

        public MvxCommand BackCommand { get; set; }
    }

    public class Group<T> : List<T>
    {


        public Group(string name, IEnumerable<T> items)
            : base(items)
        {
            this.Title = name;
        }

        public string Title
        {
            get;
            set;
        }
    }


}
