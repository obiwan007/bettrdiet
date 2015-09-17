// --------------------------------------------------------------------------------------------------------------------
// <summary>
//    Defines the FirstViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace BettrDiet.Core.ViewModels
{
    using BettrDiet.Core.Common;
    using BettrDiet.Core.Events;
    using BettrFit.Core.Common;
    using BettrFitSPA.Viewmodels.User;
    using Cirrious.CrossCore;
    using Cirrious.MvvmCross.Plugins.Messenger;
    using Cirrious.MvvmCross.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Diagnostics;
    using System.Windows.Input;

    /// <summary>
    /// Define the FirstViewModel type.
    /// </summary>
    public class MainViewModel : BaseViewModel
    {
        private BettrFitDataSource _ds;

        private bool _isNetworkAvailable = true;
        private MvxSubscriptionToken _logintoken;
        private Dictionary<string, Type> _mapNav;
        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _nonetworktoken;
        private MvxSubscriptionToken _synctoken;
        private string menuGoals = CultureHelper.GetLocalString("Goals|Ziele");
        private string menuNutrition = CultureHelper.GetLocalString("Nutrition|Nahrungsmittel");
        private string menuNutritiondiary = CultureHelper.GetLocalString("Nutrition Diary|Ernährungstagebuch");
        private string menuNutritionPlan = CultureHelper.GetLocalString("Your Nutrition Plan|Dein Ernährungsplan");
        private string menuWeight = CultureHelper.GetLocalString("Weight Diary|Gewichtstagebuch");

        private string navCommand;

        public MainViewModel()
        {
            _ds = BettrFitDataSource.Instance;
            _messenger = Mvx.Resolve<IMvxMessenger>();

            _mapNav = new Dictionary<string, Type>();
            _mapNav.Add(menuNutritionPlan, typeof(NutritionPlanOverviewViewModel));
            _mapNav.Add(menuNutritiondiary, typeof(NutritionPlanMainViewModel));
            _mapNav.Add(menuGoals, typeof(GoalOverviewViewModel));
            _mapNav.Add(menuNutrition, typeof(NutritionPlanMainViewModel));
            _mapNav.Add(menuWeight, typeof(DailyDataOverviewViewModel));

            _mapNav.Add("Login", typeof(LoginViewModel));
            _mapNav.Add("Login mit Facebook", typeof(FirstViewModel));
            _mapNav.Add(CultureHelper.GetLocalString("Register|Registrieren"), typeof(RegisterViewModel));

            NavCommands = new ObservableCollection<NavEntry>();

            NavCommand = new MvxCommand<string>(OnNavCommand);

            LogoffCommand = new MvxCommand(OnLogoffCommand);
            RefreshCommand = new MvxCommand(OnRefreshCommand);
            ShowInfoCommand = new MvxCommand(OnShowInfo);
            FeedbackCommand = new MvxCommand(OnFeedbackCommand);
            ShakeCommand = new MvxCommand(()=>_messenger.Publish(new ShakeEvent(this)));

            ShowAGBCommand = new MvxCommand(OnShowAGB);

            _logintoken = _messenger.Subscribe<LoggedInEvent>(a => LoginChanged(a.LoggedIn));
            _synctoken = _messenger.Subscribe<SyncEvent>(a => RaisePropertyChanged("Sync"));
            _nonetworktoken = _messenger.SubscribeOnMainThread<NetworkEvent>(m =>
            {
                IsNetworkAvailable = m.IsAvailable;
            });

            LoginChanged(_ds.IsLoggedIn());

            //var client = WebService.Instance.WS;

            RaisePropertyChanged("Sync");

            IsNotRefreshing = true;
        }

        public MvxCommand FeedbackCommand { get; set; }
        
        public string Image
        {
            get
            {
                if (_ds.UserData != null && _ds.UserData.Username != null)
                    return "http://www.bettrfit.com/Content/user/" + _ds.UserData.Username.Replace("@", "_").Replace(".", "_").Replace(" ", "") + ".png";
                else
                    return "";
            }
        }

        public string ImageHub
        {
            get
            {
                return "ms-appx:///Assets/Images/21315909_l.jpg";
            }
        }

        public string LatestWeight
        {
            get { var d=_ds.UserDaily.FirstOrDefault();
            return d != null ? CultureHelper.GetLocalString("your latest weight is " + d.Weight + "kg" + "|Dein letztes Gewicht war " + d.Weight + "kg") : "";
            }
        }
        

        public bool IsNetworkAvailable
        {
            get { return _isNetworkAvailable; }
            set
            {
                SetProperty(ref _isNetworkAvailable, value, () => IsNetworkAvailable);
                RaisePropertyChanged("IsNetworkNotAvailable");
            }
        }

        public bool IsNetworkNotAvailable
        {
            get { return !_isNetworkAvailable; }
        }

        public bool IsNotRefreshing { get; set; }

        public MvxCommand LogoffCommand { get; set; }
       
        public MvxCommand<string> NavCommand { get; set; }

        public ObservableCollection<NavEntry> NavCommands { get; set; }

        public MvxCommand RefreshCommand { get; set; }

        public NavEntry SelectedNav
        {
            get
            {
                return null;
            }
            set
            {
                var nav = value;
                if (nav == null)
                    return;

                navCommand = null;
                RaisePropertyChanged("SelectedNav");

                if (nav.Name != null)
                {
                    if (_mapNav[nav.Name] == typeof(NutritionPlanOverviewViewModel) && _ds.CurrentNutritionPlan == null)
                    {
                        this.ShowViewModel(typeof(NutritionPlanCreateViewModel));
                    }
                    else
                    {
                        this.ShowViewModel(_mapNav[nav.Name]);
                    }
                    //CurrentFrame.NavigationService.Navigate(new Uri("/Views/" + _mapNav[nav] + ".xaml", UriKind.Relative));
                }
            }
        }

        public MvxCommand ShowAGBCommand { get; set; }

        public MvxCommand ShowInfoCommand { get; set; }

        public bool Sync
        {
            get
            {
                Debug.WriteLine("Syncin:" + _ds._sync.IsSyncing);
                return _ds._sync.IsSyncing;
            }
        }

        public UserVM User
        {
            get
            {
                return _ds.UserData;
            }
        }

        public void LoginChanged(bool a)
        {
            NavCommands.Clear();
            if (a)
            {
                var roles = _ds.Auth.UserRoles;
                NavCommands.Add(new NavEntry(menuNutritionPlan,"appbar.book.list"));
                NavCommands.Add(new NavEntry(menuNutritiondiary, "appbar.beer"));
                //if (roles != null &&
                //        (roles.FirstOrDefault(r => r.Name == "Administrator") != null ||
                //        roles.FirstOrDefault(r => r.Name == "Trainer") != null
                //        ))
                //{
                //    NavCommands.Add((CultureHelper.GetLocalString("Workout Designer|Trainings Designer")));
                //}
                //NavCommands.Add("Auswertung");
                NavCommands.Add(new NavEntry(menuWeight, "appbar.scale.unbalanced"));
                //if (roles != null && roles.FirstOrDefault(r => r.Name == "Nutrition") != null)
                //{
                //    NavCommands.Add(CultureHelper.GetLocalString("Nutrition|Ernährung"));
                //}

                NavCommands.Add(new NavEntry(menuGoals, "appbar.stock.down"));
                //NavCommands.Add(new NavEntry("Login", "appbar.user"));
                //NavCommands.Add("Profil");
            }
            else
            {
                //_image = null;
                NavCommands.Add(new NavEntry("Login", "appbar.user"));
                //NavCommands.Add("Login mit Facebook");
                NavCommands.Add(new NavEntry(CultureHelper.GetLocalString("Register|Registrieren"), "appbar.user.add"));
            }
            RaisePropertyChanged("Image");
            RaisePropertyChanged("User");
        }

        /// <summary>
        /// Show the view model.
        /// </summary>
        public void Show()
        {
            this.ShowViewModel<FirstViewModel>();
        }

        private void OnFeedbackCommand()
        {
            this.ShowViewModel(typeof(FeedbackViewModel));
        }

        private void OnLogoffCommand()
        {
            _ds.Logout();
        }

        private void OnNavCommand(string obj)
        {
        }

        private async void OnRefreshCommand()
        {
            IsNotRefreshing = false;
            RaisePropertyChanged("IsNotRefreshing");
            await _ds.RefreshData();
            IsNotRefreshing = true;
            RaisePropertyChanged("IsNotRefreshing");
        }

        private void OnShowAGB()
        {
            _messenger.Publish<ShowAGBEvent>(new ShowAGBEvent(this));
        }

        private void OnShowInfo()
        {
            this.ShowViewModel(typeof(AboutViewModel));
        }

        public MvxCommand ShakeCommand { get; set; }
    }
}