using BettrDiet.Core.Common;
using BettrDiet.Core.Events;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BettrDiet.Core.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        public LoginViewModel()
        {
            LoginCommand = new MvxCommand(OnLogin, CanLogin);

            BackCommand = new MvxCommand(() => { this.Close(this); });

#if DEBUG
            Username= "markus.miertschink@googlemail.com";
            Pwd = "sil2con3";
#endif

            _messenger = Mvx.Resolve<IMvxMessenger>();
            _synctoken = _messenger.Subscribe<SyncEvent>(a => RaisePropertyChanged("Sync"));

        }



        private bool CanLogin()
        {
            return !_isrunning;
        }

        private async void OnLogin()
        {
            DoLogin();
        }

        public async Task<string> DoLogin()
        {
            Error = "";
            _isrunning = true;
            LoginCommand.RaiseCanExecuteChanged();
            var ret=await BettrFitDataSource.Instance.Login(Username, Pwd);
            _isrunning = false;
            if (string.IsNullOrEmpty(ret))
            {
                Error = "";
                this.Close(this);
            }
            else
            {
                Error = ret;
                
            }
            LoginCommand.RaiseCanExecuteChanged();
            return ret;
        }

        private string _error;

        public string Error
        {
            get { return _error; }
            set { 
                SetProperty(ref _error, value, () => Error);
            }
        }
        

        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _pwd;
        private bool _isrunning=false;
        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _synctoken;

        public string Pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        public bool Sync
        {
            get { return BettrFitDataSource.Instance._sync.IsSyncing; }
        }
        

        private async Task login()
        {
            //BettrFitDataSource.Instance._sync.BeginSync();
            //BettrFit.Common.WebService.Instance.Auth.Username = txtEmail.Text;
            //WebService.Instance.Auth.Password = txtPwd.Password;
            //WebService.Instance.Auth.ProviderId = 0;

            //byte[] id = (byte[])Microsoft.Phone.Info.DeviceExtendedProperties.GetValue("DeviceUniqueId");
            //string deviceID = Convert.ToBase64String(id);
            //WebService.Instance.Auth.DeviceId = deviceID;

            //var response = (await WebClientAsyncExtensions.AuthenticateUserTask(WebService.Instance.Auth));
            //WebService.Instance.Auth = response;
            //Debug.WriteLine(response);
            //BettrFitDataSource.Instance._sync.EndSync();
            //if (response.Ret == null)
            //{
            //    BettrFitDataSource.Instance.UserData.Token = response.AuthenticatedToken;

            //    BettrFitDataSource.Instance.Auth.AuthenticatedToken = response.AuthenticatedToken;
            //    BettrFitDataSource.Instance.Auth.Username = txtEmail.Text;
            //    BettrFitDataSource.Instance.Auth.UserRoles = response.UserRoles;
            //    BettrFitDataSource.Instance.Auth.UserFeatures = response.UserFeatures;
            //    BettrFitDataSource.Instance.Auth.DeviceId = deviceID;

            //    BettrFitDataSource.Instance.LoggedIn();
            //    await BettrFitDataSource.Instance.RefreshData();

            //    NavigationService.GoBack();
            //}
            //else
            //    MessageBox.Show(AppResources.Login_error,
            //    AppResources.Login_error2, MessageBoxButton.OK);


            //btnLogin.IsEnabled = true;

        }

        public MvxCommand LoginCommand { get; set; }



        public MvxCommand BackCommand { get; set; }

        public void DoClose()
        {
            this.Close(this);
        }
    }
}
