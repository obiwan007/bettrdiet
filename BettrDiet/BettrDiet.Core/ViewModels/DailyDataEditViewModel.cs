using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using BettrDiet.Core.ViewModels;


namespace BettrDiet.Core.ViewModels
{
    using System.Windows.Input;
    using Cirrious.MvvmCross.ViewModels;
    using BettrFitSPA.Viewmodels.User;
    using BettrDiet.Core.Common;
    using Cirrious.CrossCore;
    using Cirrious.MvvmCross.Plugins.Messenger;
    using BettrDiet.Core.Events;
    
    public class DailyDataEditViewModel : BaseViewModel
    {

        BettrFitDataSource _ds = BettrFitDataSource.Instance;
        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _logintoken;
        private MvxSubscriptionToken _synctoken;

        public DailyDataEditViewModel()
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
        }

        private bool canSaveDaily()
        {
                return true;
        }

        private void SaveDaily()
        {
            if (CurrentDailyData._id==null)
                _ds.AddDailyData(CurrentDailyData);
            else
                _ds.UpdateDailyData(CurrentDailyData);
            
            this.Close(this);
        }

        

        public UserDailyVM CurrentDailyData
        {
            get {
                if (_ds.CurrentDailyData == null)    // Copy Data if empty
                {
                    _ds.CurrentDailyData = new UserDailyVM();

                    var src=_ds.UserDaily.OrderByDescending(a => a.Date).FirstOrDefault();
                    if (src != null)
                    {                        
                        AutoMapper.Mapper.Map<UserDailyVM, UserDailyVM>(src, _ds.CurrentDailyData);
                    }

                    _ds.CurrentDailyData.Date = DateTime.Now;
                    _ds.CurrentDailyData._id = null;
                }
                
                return _ds.CurrentDailyData; }
            set { _ds.CurrentDailyData=value; }
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

    }
}
