using BettrDiet.Core.Events;
using BettrDiet.Core.ViewModels;
using BettrFitSPA.Viewmodels;
using BettrFitSPA.Viewmodels.User;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace BettrDiet.Core.Common
{
    /// <summary>
    /// Datastore and global access to WebService Host.
    /// </summary>
    public class BettrFitDataSource
    {
        private bool loginActive = false;

        public string Server { get; set; }

        private const string CREDENTIAL_NAME = "BETTRFIT";
        private WebService ws = WebService.Instance;
        public bool _loggedIn;
        private static BettrFitDataSource _instance;
        private IMvxMessenger _messenger;

        public SyncDataViewModel _sync { get; set; }

        public UserVM UserData { get; set; }

        public ObservableCollection<UserGoalVM> UserGoals { get; set; }

        public ObservableCollection<UserDailyVM> UserDaily { get; set; }

        public ObservableCollection<NutritionPlanFavoriteVM> NutritionPlanFavorites { get; set; }

        public ObservableCollection<LebensmittelVM> NutritionPlanLeb { get; set; }

        private ObjectStorageHelper<UserVM> userdataStore = new ObjectStorageHelper<UserVM>("User");
        private ObjectStorageHelper<WebAccess.ServiceReference.AuthData> userAuthStore = new ObjectStorageHelper<WebAccess.ServiceReference.AuthData>("AuthData");
        private ObjectStorageHelper<ObservableCollection<UserDailyVM>> userDailyStore = new ObjectStorageHelper<ObservableCollection<UserDailyVM>>("UserDailyVM");
        private ObjectStorageHelper<ObservableCollection<UserGoalVM>> userGoalStore = new ObjectStorageHelper<ObservableCollection<UserGoalVM>>("UserGoalVM");
        private ObjectStorageHelper<NutritionPlanVM> nutritionPlanStore = new ObjectStorageHelper<NutritionPlanVM>("NutritionPlan");
        private ObjectStorageHelper<ObservableCollection<NutritionPlanFavoriteVM>> nutritionFavStore = new ObjectStorageHelper<ObservableCollection<NutritionPlanFavoriteVM>>("NutritionFav");
        private ObjectStorageHelper<ObservableCollection<LebensmittelVM>> nutritionLebStore = new ObjectStorageHelper<ObservableCollection<LebensmittelVM>>("NutritionLeb");

        private InitMapper _mapper;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="nav"></param>
        /// <param name="events"></param>
        /// <param name="sync"></param>
        public BettrFitDataSource()
        {
            _sync = new SyncDataViewModel();
            Server = "https://www.bettrfit.com";
            UserData = new UserVM();
            UserGoals = new ObservableCollection<UserGoalVM>();
            Auth = new WebAccess.ServiceReference.AuthData();
            UserDaily = new ObservableCollection<UserDailyVM>();
            NutritionPlanFavorites = new ObservableCollection<NutritionPlanFavoriteVM>();
            NutritionPlanLeb = new ObservableCollection<LebensmittelVM>();
            SummaryConsumedDaytime = new WebAccess.ServiceReference.SummaryData();
            SummaryConsumedDay = new WebAccess.ServiceReference.SummaryData();

            _messenger = Mvx.Resolve<IMvxMessenger>();
            _mapper = new InitMapper();

            _messenger.Subscribe<NetworkEvent>(m => IsNetworkAvailable = m.IsAvailable);

            LoadAll();

            CheckLogin();
            //if (ret == 0)
            //{
            //    _EventAggregator = Container.Resolve<IEventAggregator>();
            //    _EventAggregator.GetEvent<LoggedInEvent>().Publish(true);
            //    _ds._loggedIn = true;
            //}
        }

        private async void CheckLogin()
        {
            _sync.BeginSync();
            try
            {
                var a = getAuth();
                if (a.AuthenticatedToken == null) // Never was here before
                {
                    _loggedIn = false;
                    Logout();
                    _messenger.Publish<LoggedInEvent>(new LoggedInEvent(this, false));
                    _sync.EndSync();
                    return;
                }
                if (IsNetworkAvailable == true)
                {
                    var ret = await WebClientAsyncExtensions.GetUserProfileTask(a);
                    AutoMapper.Mapper.Map<WebAccess.ServiceReference.UserVM, BettrFitSPA.Viewmodels.User.UserVM>(ret, UserData);

                    SaveAll();

                    _messenger.Publish<LoggedInEvent>(new LoggedInEvent(this, true));
                    Debug.WriteLine("Logged in and Saved");
                    _loggedIn = true;
                }
            }
            catch (Exception ex)
            {
                _loggedIn = false;
                Debug.WriteLine("Error in Login:" + ex);
                Logout();
                _messenger.Publish<LoggedInEvent>(new LoggedInEvent(this, false));
            }
            _sync.EndSync();
        }

        #region LOADSAVE

        /// <summary>
        /// Loads all relevant data from the local store.
        /// </summary>
        /// <returns></returns>
        internal async Task<int> LoadAll()
        {
            var s = new Stopwatch();
            s.Start();

            try
            {
                var ret = await userdataStore.LoadAsync();
                //UserData = ret.UserData;
                //Token = ret.Token;
                if (ret != null)
                {
                    UserData = ret;
                    UserData.IsChanged = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Deserialisation:" + ex);
                Logout();
                return -1;
            }
            Debug.WriteLine("Load:" + s.ElapsedMilliseconds);
            try
            {
                var ret = await userAuthStore.LoadAsync();
                //UserData = ret.UserData;
                //Token = ret.Token;
                if (ret != null)
                {
                    Auth = ret;

                    try
                    {
                        //LoggedIn();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error in loadAll:" + ex);
                    }
                }
                else
                {
                    Logout();
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Deserialisation:" + ex);
                Logout();
                return -1;
            }
            Debug.WriteLine("Load:" + s.ElapsedMilliseconds);

            try
            {
                var ret = await userGoalStore.LoadAsync();
                if (ret != null)
                {
                    UserGoals.Clear();
                    foreach (var e in ret)
                    {
                        e.IsChanged = false;
                        UserGoals.Add(e);
                    }
                    //AutoMapper.Mapper.Map<PlannedWorkoutVM, PlannedWorkoutVM>(ret, CurrentPlannedWorkout);

                    Debug.WriteLine("Deserialisation Goals finished");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Deserialisation UserGoals:" + ex);
            }

            try
            {
                var ret = await userDailyStore.LoadAsync();
                if (ret != null)
                {
                    UserDaily.Clear();
                    foreach (var e in ret)
                    {
                        e.IsChanged = false;
                        UserDaily.Add(e);
                    }
                    //AutoMapper.Mapper.Map<PlannedWorkoutVM, PlannedWorkoutVM>(ret, CurrentPlannedWorkout);

                    Debug.WriteLine("Deserialisation UserDaily finished");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Deserialisation UserDaily:" + ex);
            }

            try
            {
                var ret = await nutritionPlanStore.LoadAsync();
                if (ret != null)
                {
                    CurrentNutritionPlan = AutoMapper.Mapper.Map<NutritionPlanVM, NutritionPlanVM>(ret);
                    Debug.WriteLine("Deserialisation nutritionPlanStore finished");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Deserialisation nutritionPlanStore:" + ex);
            }

            try
            {
                var ret = await nutritionFavStore.LoadAsync();
                if (ret != null)
                {
                    NutritionPlanFavorites.Clear();
                    foreach (var e in ret)
                    {
                        NutritionPlanFavorites.Add(e);
                    }
                    Debug.WriteLine("Deserialisation nutritionFavStore finished");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Deserialisation nutritionFavStore:" + ex);
            }

            try
            {
                var ret = await nutritionLebStore.LoadAsync();
                if (ret != null)
                {
                    NutritionPlanLeb.Clear();
                    foreach (var e in ret)
                    {
                        NutritionPlanLeb.Add(e);
                    }
                    Debug.WriteLine("Deserialisation nutritionLebStore finished");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in Deserialisation nutritionLebStore:" + ex);
            }

            _loggedIn = true;
            _messenger.Publish<LoggedInEvent>(new LoggedInEvent(this, true));
            return 0;
        }

        /// <summary>
        /// Save all Data to the local store.
        /// </summary>
        public void SaveAll()
        {
            userdataStore.SaveAsync(UserData);
            userAuthStore.SaveAsync(Auth);
            userGoalStore.SaveAsync(UserGoals);
            userDailyStore.SaveAsync(UserDaily);

            nutritionLebStore.SaveAsync(NutritionPlanLeb);
            nutritionFavStore.SaveAsync(NutritionPlanFavorites);
            nutritionPlanStore.SaveAsync(CurrentNutritionPlan);
        }

        /// <summary>
        /// Save all Data to the local store.
        /// </summary>
        private void DeleteAll()
        {
            userdataStore.DeleteAsync(UserData);
            userAuthStore.DeleteAsync(Auth);
            userGoalStore.DeleteAsync(UserGoals);
            userDailyStore.DeleteAsync(UserDaily);

            nutritionPlanStore.DeleteAsync(CurrentNutritionPlan);
            nutritionFavStore.DeleteAsync();
            nutritionLebStore.DeleteAsync();
        }

        /// <summary>
        /// Refresh all internal Data frm WebServices
        /// </summary>
        /// <returns></returns>
        public async Task<int> RefreshData()
        {
            ////FillDailyData();

            _sync.BeginSync();
            if (IsNetworkAvailable == true)
            {
                await FillDailyData();
                //    fillAchievment();

                await FillGoals();
                //    BettrFitDataSource.Instance.FillCurrentConsumedSummary(BettrFitDataSource.Instance.SummaryConsumedDay);
                //}

                var p = await FillCurrentNutritionPlan();

                if (p != null)
                {
                    await FillNutritionPlanFavorites(p._id);
                    Debug.WriteLine("Favorites geladen");
                    await FillNutritionPlanLebensmittel(p.PlanType);
                    Debug.WriteLine("Lebensmittel geladen:" + NutritionPlanLeb.Count);
                }

                SaveAll();
            }
            _sync.EndSync();
            return 0;
        }

        #endregion LOADSAVE

        #region AUTH

        /// <summary>
        /// Gives back the valid Auth Token used to communicate with WebService.
        /// </summary>
        /// <returns></returns>
        public WebAccess.ServiceReference.AuthData getAuth()
        {
            var auth = new WebAccess.ServiceReference.AuthData();
            auth.AuthenticatedToken = Auth.AuthenticatedToken;
            auth.Username = Auth.Username;
            return auth;
        }

        public WebAccess.ServiceReference.AuthData Auth { get; set; }

        #endregion AUTH

        #region LOGINHANDLING

        /// <summary>
        /// Checks if the User is logged in or not.
        /// Checks if a credential is stored in the credentialcache
        /// </summary>
        /// <returns></returns>
        public bool IsLoggedIn()
        {
            return _loggedIn;
        }

        /// <summary>
        /// Logout of system. Will ensure that the credentialcache is deleted and all stored data is cleared.
        /// </summary>
        internal void Logout()
        {
            Auth.AuthenticatedToken = null;
            UserGoals.Clear();
            UserData.Username = null;
            UserData.Nickname = "";
            UserDaily.Clear();
            DeleteAll();

            _messenger.Publish<LoggedInEvent>(new LoggedInEvent(this, false));
        }

        /// <summary>
        /// Called after a successfull Login was detected and initializes all necessary data via loading it from WebService
        /// </summary>
        /// <returns></returns>
        public async Task<bool> LoggedIn()
        {
            try
            {
                var a = getAuth();
                var ret = await WebClientAsyncExtensions.GetUserProfileTask(a);
                AutoMapper.Mapper.Map<WebAccess.ServiceReference.UserVM, BettrFitSPA.Viewmodels.User.UserVM>(ret, UserData);

                await RefreshData();

                SaveAll();

                _messenger.Publish<LoggedInEvent>(new LoggedInEvent(this, true));
                Debug.WriteLine("Logged in and Saved");
                _loggedIn = true;
            }
            catch (Exception ex)
            {
                _loggedIn = false;
                Debug.WriteLine("Error in Login:" + ex);
                Logout();
                _messenger.Publish<LoggedInEvent>(new LoggedInEvent(this, false));
            }
            return _loggedIn;
        }

        public async Task<string> Login(string user, string pwd)
        {
            BettrFitDataSource.Instance._sync.BeginSync();
            WebService.Instance.Auth.Username = user;
            WebService.Instance.Auth.Password = pwd;
            WebService.Instance.Auth.ProviderId = 0;

            //string deviceID = Convert.ToBase64String(id);
            WebService.Instance.Auth.DeviceId = "0000";

            //var response

            var response = (await WebClientAsyncExtensions.AuthenticateUserTask(WebService.Instance.Auth));

            WebService.Instance.Auth = response;
            Debug.WriteLine(response);
            BettrFitDataSource.Instance._sync.EndSync();
            if (response != null)
            {
                if (!string.IsNullOrEmpty(response.Ret))
                    return response.Ret;
            }
            if (response != null)
            {
                BettrFitDataSource.Instance.UserData.Token = response.AuthenticatedToken;

                BettrFitDataSource.Instance.Auth.AuthenticatedToken = response.AuthenticatedToken;
                BettrFitDataSource.Instance.Auth.Username = user;
                BettrFitDataSource.Instance.Auth.UserRoles = response.UserRoles;
                BettrFitDataSource.Instance.Auth.UserFeatures = response.UserFeatures;
                BettrFitDataSource.Instance.Auth.DeviceId = "0000";

                await LoggedIn();
                await BettrFitDataSource.Instance.RefreshData();

                return "";
            }
            else
            {
                return "Error";
            }
        }

        #endregion LOGINHANDLING

        #region DAILYDATA

        /// <summary>
        /// Retrieve Daily Data from WebService
        /// </summary>
        /// <returns></returns>
        public async Task<int> FillDailyData()
        {
            int ret = 0;
            if (IsNetworkAvailable == true)
            {
                _sync.BeginSync();
                try
                {
                    UserDaily.Clear();

                    var a = getAuth();
                    var count = await WebClientAsyncExtensions.GetUserDailyCountTask(a);
                    for (int i = 0; i < count; i += 100)
                    {
                        var gl = await WebClientAsyncExtensions.GetUserDailyRangeTask(a, i, 100);

                        foreach (var g in gl)
                        {
                            var gvm = AutoMapper.Mapper.Map<WebAccess.ServiceReference.UserDailyVM, UserDailyVM>(g);
                            gvm.IsChanged = false;
                            if (gvm.Weight > 200 || gvm.Weight == 0)
                                continue;
                            UserDaily.Add(gvm);
                        }
                    }
                    Debug.WriteLine("FillDailyData:" + count);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in finish current TrainingDay:" + ex);
                    //Alert(ex.Message);
                }
                //await userDailyStore.SaveAsync(UserDaily);
                _sync.EndSync();
            }
            //_EventAggregator.GetEvent<UserDailyEvent>().Publish(true);
            return ret;
        }

        /// <summary>
        /// Save all changed Daily Data entries.
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveDailyData()
        {
            int ret = 0;
            //_sync.BeginSync();
            //try
            //{
            //    var data = UserDaily.Where(a => a.IsChanged);
            //    var auth = getAuth();
            //    Debug.WriteLine("Update UserDaily:" + data.Count());
            //    foreach (var d in data)
            //    {
            //        var dvm = AutoMapper.Mapper.Map<UserDailyVM, WebAccess.ServiceReference.UserDailyVM>(d);
            //        var retval = await ws.WS.UpdateUserDailyAsync(auth, dvm);
            //        if (!string.IsNullOrEmpty(retval.Body.UpdateUserDailyResult))
            //        {
            //            var newdata = await ws.WS.GetUserDailyAsync(auth, retval.Body.UpdateUserDailyResult);
            //            AutoMapper.Mapper.Map<WebAccess.ServiceReference.UserDailyVM, UserDailyVM>(newdata.Body.GetUserDailyResult, d);
            //            d.IsChanged = false;
            //        }
            //    }
            //    //await userDailyStore.SaveAsync(UserDaily);
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("Error in SaveDailyData:" + ex);
            //    //Alert(ex.Message);
            //}

            //_sync.EndSync();
            //_EventAggregator.GetEvent<UserDailyEvent>().Publish(true);
            return ret;
        }

        /// <summary>
        /// Removes a specified Entry from List and Remote System via WebService
        /// </summary>
        /// <param name="Selected"></param>
        /// <returns></returns>
        internal async Task<int> DeleteDailyData(UserDailyVM Selected)
        {
            if (IsNetworkAvailable == false)
            {
                return -1;
            }

            int ret = 0;

            try
            {
                UserDaily.Remove(Selected);
                if (Selected._id == null)
                    return 0;

                _sync.BeginSync();

                try
                {
                    var auth = getAuth();
                    var retval = await WebClientAsyncExtensions.DeleteUserDailyTask(auth, Selected._id);

                    if (retval == 0)
                    {
                        UserDaily.Remove(Selected);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error in DeleteDailyData:" + ex);
                    //Alert(ex.Message);
                    ret = -1;
                }
                await userDailyStore.SaveAsync(UserDaily);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in finish current DeleteDailyData:" + ex);
                //Alert(ex.Message);
            }
            _sync.EndSync();

            return ret;
        }

        internal async void AddDailyData(UserDailyVM daily)
        {
            if (IsNetworkAvailable == false)
            {
                return;
            }
            _sync.BeginSync();
            try
            {
                var auth = getAuth();
                var dvm = AutoMapper.Mapper.Map<UserDailyVM, WebAccess.ServiceReference.UserDailyVM>(daily);
                var retval = await WebClientAsyncExtensions.AddUserDailyTask(auth, dvm);
                daily._id = retval;

                var dailyNew = await WebClientAsyncExtensions.GetUserDailyTask(auth, retval);
                AutoMapper.Mapper.Map<WebAccess.ServiceReference.UserDailyVM, UserDailyVM>(dailyNew, daily);
                UserDaily.Insert(0, daily);
                await userDailyStore.SaveAsync(UserDaily);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in SaveDailyData:" + ex);
                //Alert(ex.Message);
            }

            _sync.EndSync();
        }

        internal async void UpdateDailyData(UserDailyVM daily)
        {
            if (IsNetworkAvailable == false)
            {
                return;
            }
            _sync.BeginSync();
            try
            {
                var auth = getAuth();
                var dvm = AutoMapper.Mapper.Map<UserDailyVM, WebAccess.ServiceReference.UserDailyVM>(daily);
                var retval = await WebClientAsyncExtensions.UpdateUserDailyTask(auth, dvm);
                daily._id = retval;
                var dailyNew = await WebClientAsyncExtensions.GetUserDailyTask(auth, retval);
                AutoMapper.Mapper.Map<WebAccess.ServiceReference.UserDailyVM, UserDailyVM>(dailyNew, daily);
                await userDailyStore.SaveAsync(UserDaily);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in SaveDailyData:" + ex);
                //Alert(ex.Message);
            }

            _sync.EndSync();
        }

        #endregion DAILYDATA

        #region Registrations

        public async Task<int> RegisterNewUser(WebAccess.ServiceReference.RegistrationItem vm)
        {
            _sync.BeginSync();
            try
            {
                var reg = await WebClientAsyncExtensions.RegisterNewUserTask(vm);

                if (reg == 0)
                {
                    var a = new WebAccess.ServiceReference.AuthData();
                    a.Username = vm.Email;
                    a.Password = vm.Pwd1;
                    a.ProviderId = 0;
                    var t = await WebClientAsyncExtensions.AuthenticateUserTask(a);

                    if (t != null)
                    {
                        Auth = t;
                        Auth.Username = vm.Email;
                        //_EventAggregator.GetEvent<LoggedInEvent>().Publish(true);

                        await LoggedIn();
                        _sync.EndSync();
                        return 0;
                    }
                    else
                    {
                        _messenger.Publish<LoggedInEvent>(new LoggedInEvent(this, false));
                        _sync.EndSync();
                        return -2;
                    }
                }
                else
                {
                    _messenger.Publish<LoggedInEvent>(new LoggedInEvent(this, false));
                    _sync.EndSync();
                    return reg;
                }
            }
            catch (Exception ex)
            {
                _sync.EndSync();
                return -3;
            }
        }

        public int RegisterStep { get; set; }

        #endregion Registrations

        #region USERDATA

        /// <summary>
        /// Sende Passwort Reset.
        /// Token wird per email gesendet...
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal async Task<bool> SendPasswordReset(string p)
        {
            var ret = false;
            //_sync.BeginSync();
            //try
            //{
            //    var a = await ws.WS.ResetUserAsync(p);
            //    ret = true;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("SendPasswordReset:" + ex);
            //}
            //_sync.EndSync();

            return ret;
        }

        /// <summary>
        /// Send a Feedback string to the system
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal async Task<int> SendFeedback(string p)
        {
            var ret = -1;
            //_sync.BeginSync();
            //try
            //{
            //    var a =
            //    await ws.WS.SendFeedbackAsync(getAuth(), p);
            //    ret = 0;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("SendFeedback:" + ex);
            //}
            //_sync.EndSync();

            return ret;
        }

        internal async Task<string> UploadPicture(byte[] imageBytes)
        {
            var ret = "";
            //_sync.BeginSync();
            //try
            //{
            //    var a = getAuth();
            //    ret = (await ws.WS.UploadProfilePictureAsync(a, imageBytes)).Body.UploadProfilePictureResult;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("UploadPicture:" + ex);
            //    //Alert(ex.Message);
            //}
            //_sync.EndSync();
            return ret;
        }

        internal async Task<int> SaveUserData()
        {
            int ret = 0;
            //_sync.BeginSync();
            //try
            //{
            //    var day = AutoMapper.Mapper.Map<UserVM, WebAccess.ServiceReference.UserVM>(UserData);
            //    var a = getAuth();
            //    var s1 = new Stopwatch();
            //    s1.Start();
            //    var resp = await ws.WS.UpdateUserProfileAsync(a, day);
            //    s1.Stop();
            //    Debug.WriteLine("Time:" + s1.ElapsedMilliseconds);

            //    AutoMapper.Mapper.Map<WebAccess.ServiceReference.UserVM, UserVM>(resp.Body.UpdateUserProfileResult, UserData);
            //    UserData.IsChanged = false;
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("SaveUserData:" + ex);
            //    //Alert(ex.Message);
            //}
            //try
            //{
            //    //userdataStore.SaveAsync(UserData);
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("Error saving Userdata:" + ex);
            //}

            _sync.EndSync();
            //userdataStore.SaveAsync(UserData);
            return ret;
        }

        [DataContractAttribute]
        public class UserInfo
        {
            [DataMember]
            public string email { get; set; }

            [DataMember]
            public string id { get; set; }
        }

        /// <summary>
        /// Updates the Achievment via Updating of UserData
        /// </summary>
        private async void updateAchievments()
        {
            //try
            //{
            //    Debug.WriteLine("Update Achievment!");
            //    var a = getAuth();
            //    var ret = (await ws.WS.GetUserProfileAsync(a)).Body.GetUserProfileResult;
            //    AutoMapper.Mapper.Map<BettrFitServiceReference.UserVM, UserVM>(ret, UserData);
            //    //userdataStore.SaveAsync(UserData);
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("Error in Updating Achievment:" + ex);
            //}
        }

        #endregion USERDATA

        #region CONTENT

        internal async Task<BlogPostVM> GetContent(string id)
        {
            BlogPostVM ret = new BlogPostVM();
            //_sync.BeginSync();
            //try
            //{
            //    var lang = CultureHelper.GetCurrentCulture().Substring(0, 2);
            //    var blog = (await ws.WS.GetContentAsync(id, lang)).Body.GetContentResult;

            //    AutoMapper.Mapper.Map<WebAccess.ServiceReference.BlogPostVM, BlogPostVM>(blog, ret);

            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("GetContent:" + ex);
            //    //Alert(ex.Message);
            //}
            //_sync.EndSync();
            return ret;
        }

        #endregion CONTENT

        #region FOOD

        public async Task<int> SearchConsumed(int daytime, DateTime date, ObservableCollection<LebensmittelConsumedVM> col)
        {
            if (IsNetworkAvailable == false)
            {
                return -1;
            }
            _sync.BeginSync();
            var result = await WebClientAsyncExtensions.GetConsumedTask(getAuth(), daytime, date);
            try
            {
                foreach (var i in result.ToList())
                {
                }
                foreach (var i in result.ToList())
                {
                    try
                    {
                        LebensmittelConsumedVM m;
                        var item = col.FirstOrDefault(aa => aa._id == i._id);
                        if (item != null)
                        {
                            m = AutoMapper.Mapper.Map<WebAccess.ServiceReference.LebensmittelConsumedVM, LebensmittelConsumedVM>(i);
                            item = m;
                        }
                        else
                        {
                            m = AutoMapper.Mapper.Map<WebAccess.ServiceReference.LebensmittelConsumedVM, LebensmittelConsumedVM>(i);
                            col.Add(m);
                        }
                        m.Lebensmittel = NutritionPlanLeb.FirstOrDefault(la => la._id == m.Lebensmittel_id);
                        m.Lebensmittel_Image = "http://www.bettrfit.com/Content/food/" + m.Lebensmittel._id + ".png";

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error:" + ex);
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error:" + ex);
            }
            SelectedConsumed = col.FirstOrDefault();

            var summary = await WebClientAsyncExtensions.GetSummaryTask(getAuth(), -1, date);
            var a = summary;
            FillConsumedSummary(a, SummaryConsumedDay);
            summary = await WebClientAsyncExtensions.GetSummaryTask(getAuth(), daytime, date);
            a = summary;
            FillConsumedSummary(a, SummaryConsumedDaytime);
            FillMacroList(SummaryConsumedDay);
            _sync.EndSync();
            return 0;
        }

        private void FillConsumedSummary(WebAccess.ServiceReference.SummaryData source, WebAccess.ServiceReference.SummaryData dest)
        {
            dest.Fiber = source.Fiber;
            dest.SumFat = source.SumFat;
            dest.SumFatP = source.SumFatP;
            dest.SumFatSaturated = source.SumFatSaturated;
            dest.SumkCal = source.SumkCal;
            dest.SumKH = source.SumKH;
            dest.SumKHP = source.SumKHP;
            dest.SumkJoule = source.SumkJoule;
            dest.SumProt = source.SumProt;
            dest.SumProtP = source.SumProtP;
        }

        private ObservableCollection<Double> _macros = new ObservableCollection<double>();

        public ObservableCollection<Double> Macros
        {
            get { return _macros; }
            set { _macros = value; }
        }

        private void FillMacroList(WebAccess.ServiceReference.SummaryData summary)
        {
            Macros.Clear();
            Macros.Add(summary.SumFat * 9);
            Macros.Add(summary.SumKH * 4);
            Macros.Add(summary.SumProt * 4);
        }

        public async Task<int> GetConsumed(int daytime, DateTime date, ObservableCollection<LebensmittelConsumedVM> col)
        {
            _sync.BeginSync();

            //try
            //{
            //    var result = await WebClientAsyncExtensions.GetConsumedTask(getAuth(), daytime, date);

            //    foreach (var i in result.Body.GetConsumedResult)
            //    {
            //        var c = AutoMapper.Mapper.Map<BettrFitServiceReference.LebensmittelConsumedVM, LebensmittelConsumedVM>(i);
            //        c.IsChanged = false;
            //        col.Add(c);
            //    }
            //    var idlist = col.Where(a => a.Lebensmittel == null).Select(a => a.Lebensmittel_id);
            //    Debug.WriteLine(idlist.Count());
            //    var l2 = new BettrFit.WindowsStore.BettrFitServiceReference.ArrayOfString();
            //    foreach (var entry in idlist)
            //    {
            //        l2.Add(entry);
            //    }
            //    var leb = await WebService.Instance.WS.GetFoodListAsync(getAuth(), l2);

            //    foreach (var l in leb.Body.GetFoodListResult)
            //    {
            //        var lvm = col.FirstOrDefault(a => a.Lebensmittel_id == l._id && a.Lebensmittel == null);
            //        if (lvm != null)
            //            lvm.Lebensmittel = AutoMapper.Mapper.Map<BettrFitServiceReference.LebensmittelVM, LebensmittelVM>(l);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("Error in GetConsumed ex:" + ex);
            //}
            _sync.EndSync();
            return 0;
        }

        public async Task<int> RefreshConsumed(int daytime, DateTime date, ObservableCollection<LebensmittelConsumedVM> col)
        {
            _sync.BeginSync();
            //try
            //{
            //    var result = await WebService.Instance.WS.GetConsumedAsync(getAuth(), daytime, date);

            //    foreach (var i in result.Body.GetConsumedResult)
            //    {
            //        var c = AutoMapper.Mapper.Map<BettrFitServiceReference.LebensmittelConsumedVM, LebensmittelConsumedVM>(i);
            //        c.IsChanged = false;
            //        if (col.FirstOrDefault(a => a._id == c._id) == null)
            //            col.Add(c);
            //    }
            //    var idlist = col.Where(a => a.Lebensmittel == null).Select(a => a.Lebensmittel_id);
            //    Debug.WriteLine(idlist.Count());
            //    var l2 = new BettrFit.WindowsStore.BettrFitServiceReference.ArrayOfString();
            //    foreach (var entry in idlist)
            //    {
            //        l2.Add(entry);
            //    }
            //    var leb = await WebService.Instance.WS.GetFoodListAsync(getAuth(), l2);

            //    foreach (var l in leb.Body.GetFoodListResult)
            //    {
            //        var lvm = col.FirstOrDefault(a => a.Lebensmittel_id == l._id && a.Lebensmittel == null);
            //        if (lvm != null)
            //            lvm.Lebensmittel = AutoMapper.Mapper.Map<BettrFitServiceReference.LebensmittelVM, LebensmittelVM>(l);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Debug.WriteLine("Error in RefreshConsumed ex:" + ex);
            //}
            _sync.EndSync();
            return 0;
        }

        //public async Task<int> SearchLebensmittel(string search, ObservableCollection<LebensmittelVM> col)
        //{
        //    _sync.BeginSync();
        //    try
        //    {
        //        var a = getAuth();
        //        var result = await WebClientAsyncExtensions.SearchFood(a, search, true, 0);
        //        col.Clear();
        //        foreach (var i in result.Body.SearchFoodResult)
        //        {
        //            col.Add(AutoMapper.Mapper.Map<BettrFitServiceReference.LebensmittelVM, LebensmittelVM>(i));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    _sync.EndSync();
        //    return 0;
        //}

        public async Task<LebensmittelVM> GetLebensmittel(string id)
        {
            _sync.BeginSync();
            try
            {
                var a = getAuth();
            }
            catch (Exception ex)
            {
            }
            _sync.EndSync();
            return null;
        }

        public async Task<LebensmittelConsumedVM> AddConsumedAsync(string id, float menge, int daytime, DateTime date)
        {
            if (IsNetworkAvailable == false)
            {
                return null;
            }
            LebensmittelConsumedVM vm = null;
            _sync.BeginSync();
            try
            {
                var ret = await WebClientAsyncExtensions.AddConsumedTask(getAuth(), id, menge, daytime, date);
                vm = AutoMapper.Mapper.Map<WebAccess.ServiceReference.LebensmittelConsumedVM, LebensmittelConsumedVM>(ret);

                var leb = await WebClientAsyncExtensions.GetFoodTask(getAuth(), vm.Lebensmittel_id);
                var lebvm = AutoMapper.Mapper.Map<WebAccess.ServiceReference.LebensmittelVM, LebensmittelVM>(leb);
                vm.Lebensmittel_id = lebvm._id;
                vm.Name = lebvm.Name;
            }
            catch (Exception ex)
            {
            }
            _sync.EndSync();
            return vm;
        }

        public async Task<int> SetConsumedAsync(LebensmittelConsumedVM consumed)
        {
            if (IsNetworkAvailable == false)
            {
                return -1;
            }
            _sync.BeginSync();
            try
            {
                var vm = AutoMapper.Mapper.Map<LebensmittelConsumedVM, WebAccess.ServiceReference.LebensmittelConsumedVM>(consumed);
                var ret = await WebClientAsyncExtensions.SetConsumedTask(getAuth(), vm);
            }
            catch (Exception ex)
            {
            }
            _sync.EndSync();
            return 0;
        }

        public async Task<int> GetSummary(int daytime, DateTime date, WebAccess.ServiceReference.SummaryData _summary)
        {
            if (IsNetworkAvailable == false)
            {
                return -1;
            }
            _sync.BeginSync();
            try
            {
                var summary = (await WebClientAsyncExtensions.GetSummaryTask(getAuth(), daytime, date));

                AutoMapper.Mapper.Map<WebAccess.ServiceReference.SummaryData, WebAccess.ServiceReference.SummaryData>(summary, _summary);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in getting Consumed Summary Data:" + ex);
            }
            _sync.EndSync();

            return 0;
        }

        public async Task<int> DeleteConsumed(string id)
        {
            if (IsNetworkAvailable == false)
            {
                return -1;
            }
            _sync.BeginSync();
            try
            {
                await WebClientAsyncExtensions.DeleteConsumedTask(getAuth(), id);
            }
            catch (Exception ex)
            {
            }
            _sync.EndSync();
            return 0;
        }

        public async Task<ObservableCollection<NutritionPlanFavoriteVM>> FillNutritionPlanFavorites(string plan)
        {
            if (IsNetworkAvailable == false)
            {
                return null;
            }
            _sync.BeginSync();
            try
            {
                var a = getAuth();
                var result = await WebClientAsyncExtensions.GetNutritionPlanFavoritesTask(a, plan);
                NutritionPlanFavorites.Clear();
                foreach (var i in result)
                {
                    NutritionPlanFavorites.Add(AutoMapper.Mapper.Map<WebAccess.ServiceReference.NutritionPlanFavoriteVM, NutritionPlanFavoriteVM>(i));
                }
            }
            catch (Exception ex)
            {
            }
            _sync.EndSync();
            return NutritionPlanFavorites;
        }

        public async Task<ObservableCollection<LebensmittelVM>> FillNutritionPlanLebensmittel(string plan)
        {
            if (IsNetworkAvailable == false)
            {
                return null;
            }
            _sync.BeginSync();
            try
            {
                var a = getAuth();
                var result = await WebClientAsyncExtensions.GetNutritionPlanLebensmittelTask(a, plan);
                NutritionPlanLeb.Clear();
                foreach (var i in result)
                {
                    var m = AutoMapper.Mapper.Map<WebAccess.ServiceReference.LebensmittelVM, LebensmittelVM>(i);
                    m.Image = "http://bettrfit.com/Content/food/" + m._id + ".png";

                    m.CurrentPlanType = m.PlanEntries.FirstOrDefault(p => p.Plan == CurrentNutritionPlan.PlanType).PlanTag;

                    NutritionPlanLeb.Add(m);
                }
            }
            catch (Exception ex)
            {
            }
            _sync.EndSync();
            return NutritionPlanLeb;
        }

        public async Task<NutritionPlanVM> FillCurrentNutritionPlan()
        {
            if (IsNetworkAvailable == false)
            {
                return null;
            }
            _sync.BeginSync();
            try
            {
                var a = getAuth();
                var result = await WebClientAsyncExtensions.GetMyCurrentNutritionPlanTask(a);
                CurrentNutritionPlan = AutoMapper.Mapper.Map<WebAccess.ServiceReference.NutritionPlanVM, NutritionPlanVM>(result);
            }
            catch (Exception ex)
            {
            }
            _sync.EndSync();
            return CurrentNutritionPlan;
        }

        public async Task<NutritionPlanVM> GetNewNutritionPlan(string plan)
        {
            if (IsNetworkAvailable == false)
            {
                return null;
            }
            _sync.BeginSync();
            try
            {
                var a = getAuth();
                var result = await WebClientAsyncExtensions.GetMyCurrentNutritionPlanTask(a);
                CurrentNutritionPlan = AutoMapper.Mapper.Map<WebAccess.ServiceReference.NutritionPlanVM, NutritionPlanVM>(result);
            }
            catch (Exception ex)
            {
            }
            _sync.EndSync();
            return CurrentNutritionPlan;
        }

        public async Task<Plan1PointsVM> GetRemainingPlan1Points(DateTime date)
        {
            if (IsNetworkAvailable == false)
            {
                return null;
            }
            _sync.BeginSync();
            try
            {
                var a = getAuth();
                var result = await WebClientAsyncExtensions.GetRemainingPlan1PointsTask(a, date);
                _sync.EndSync();
                return AutoMapper.Mapper.Map<WebAccess.ServiceReference.Plan1PointsVM, Plan1PointsVM>(result);
            }
            catch (Exception ex)
            {
            }
            _sync.EndSync();
            return null;
        }

        public void CalcBMR(UserVM user, ref double bmr, ref double act, ref double weight, ref string answerstring)
        {
            var daily = UserDaily.OrderByDescending(a => a.Date).FirstOrDefault();
            weight = (float)daily.Weight;
            var height = (float)user.Height;
            var age = Math.Round((DateTime.Now - user.Birthday).TotalDays / 365);

            var goal = UserGoals.FirstOrDefault();
            var days = goal.WorkoutDays;
            var isMale = user.Gender == "M" ? true : false;

            if (isMale == true)
            {
                bmr = 66 + (13.7 * weight) + (5 * height) - (6.8 * age);
            }
            else
            {
                bmr = 655 + (9.6 * weight) + (1.8 * height) - (4.7 * age);
            }

            string intens = "";

            if (days < 1)
            {
                act = bmr * 1.2;
                intens = "geringer";
            }
            else if (days < 3)
            {
                act = bmr * 1.375;
                intens = "mittlerer";
            }
            else if (days < 5)
            {
                act = bmr * 1.55;
                intens = "hoher";
            }
            else if (days < 7)
            {
                act = bmr * 1.725;
                intens = "sehr hoher";
            }

            answerstring = "Du bist " + age + " Jahre alt, wiegst " + Math.Round(weight) + "kg und bist " + Math.Round(height) + "cm groß. Du bewegst dich den Tag über mit " + intens + " Intensität.";
            act = Math.Round(act, 0);
            bmr = Math.Round(bmr, 0);
            return;
        }

        #endregion FOOD

        public UserDailyVM CurrentDailyData { get; set; }

        public static BettrFitDataSource Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BettrFitDataSource();
                return _instance;
            }
        }

        #region Goals

        /// <summary>
        /// Load Goals
        /// </summary>
        /// <returns></returns>
        internal async Task<int> FillGoals()
        {
            int ret = 0;
            _sync.BeginSync();
            try
            {
                UserGoals.Clear();
                var a = getAuth();
                var resp = await WebClientAsyncExtensions.GetUserGoalCountTask(a);

                var goalcount = resp;
                for (int i = 0; i < goalcount; i += 20)
                {
                    var gl = await WebClientAsyncExtensions.GetUseGoalRangeTask(a, i, 20);

                    foreach (var g in gl)
                    {
                        var gvm = AutoMapper.Mapper.Map<WebAccess.ServiceReference.UserGoalVM, UserGoalVM>(g);
                        gvm.IsChanged = false;
                        UserGoals.Add(gvm);
                    }
                }
                //await userGoalStore.SaveAsync(UserGoals);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in FillGoals:" + ex);
                //Alert(ex.Message);
            }

            _sync.EndSync();

            return ret;
        }

        internal async void AddUserGoal(UserGoalVM userGoal)
        {
            _sync.BeginSync();
            try
            {
                var a = getAuth();
                var m = AutoMapper.Mapper.Map<UserGoalVM, WebAccess.ServiceReference.UserGoalVM>(userGoal);
                var i = (await WebClientAsyncExtensions.AddUserGoalTask(a, m));
                if (!string.IsNullOrEmpty(i))
                {
                    await FillGoals();
                    //userGoal._id = i;
                    //UserGoals.Insert(0, userGoal);

                    userGoalStore.SaveAsync(UserGoals);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Error Adding Usergoal");
            }
            _sync.EndSync();
        }

        internal async void UpdateUserGoal(UserGoalVM userGoal)
        {
            _sync.BeginSync();
            try
            {
                var a = getAuth();
                var m = AutoMapper.Mapper.Map<UserGoalVM, WebAccess.ServiceReference.UserGoalVM>(userGoal);
                var i = (await WebClientAsyncExtensions.UpdateUserGoalTask(a, m));

                await FillGoals();
                userGoalStore.SaveAsync(UserGoals);
            }
            catch (Exception)
            {
                Debug.WriteLine("Error Update Usergoal");
            }
            _sync.EndSync();
        }

        internal async void DeleteUserGoal(UserGoalVM userGoal)
        {
            _sync.BeginSync();
            try
            {
                var a = getAuth();
                var m = AutoMapper.Mapper.Map<UserGoalVM, WebAccess.ServiceReference.UserGoalVM>(userGoal);
                var i = (await WebClientAsyncExtensions.DeleteUserGoalTask(a, m._id));
                if (i == 0)
                {
                    var d = UserGoals.FirstOrDefault(z => z._id == userGoal._id);
                    if (d != null)
                        UserGoals.Remove(d);

                    userGoalStore.SaveAsync(UserGoals);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Error Delete Usergoal");
            }
            _sync.EndSync();
        }

        public UserGoalVM SelectedUserGoal { get; set; }

        #endregion Goals

        public NutritionPlanVM CurrentNutritionPlan { get; set; }

        public LebensmittelVM CurrentLebensmittel { get; set; }

        public WebAccess.ServiceReference.SummaryData SummaryConsumedDaytime { get; set; }

        public WebAccess.ServiceReference.SummaryData SummaryConsumedDay { get; set; }

        public LebensmittelConsumedVM SelectedConsumed { get; set; }

        private bool _isNetworkAvailable = true;

        public bool IsNetworkAvailable
        {
            get { return _isNetworkAvailable; }
            set
            {
                if (_isNetworkAvailable != value)
                {
                    _isNetworkAvailable = value;

                    if (_isNetworkAvailable == false)
                    {
                        _messenger.Publish<PopupEvent>(new PopupEvent(this, "Netzwerk ist momentan nicht erreichbar.", "Fehler"));
                    }
                }
            }
        }

        public UserGoalVM CurrentGoal { get; set; }
    }
}