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
    using System.Xml.Linq;


    /// <summary>
    /// Define the FirstViewModel type.
    /// </summary>
    public class FeedbackViewModel : BaseViewModel
    {
        BettrFitDataSource _ds;

        public FeedbackViewModel()
        {
            _ds = BettrFitDataSource.Instance;

            SendCommand = new MvxCommand(OnSend);

            _messenger = Mvx.Resolve<IMvxMessenger>();
            _synctoken = _messenger.Subscribe<SyncEvent>(a => RaisePropertyChanged("Sync"));

            //var client = WebService.Instance.WS;

            RaisePropertyChanged("Sync");
        }

        private async void OnSend()
        {
            if (Text.Length > 5)
            {
                BettrFitDataSource.Instance._sync.BeginSync();
                try
                {
                    XDocument doc = XDocument.Load("WMAppManifest.xml");
                    var app = doc.Root.Element("App");
                    var vers = app.Attribute("Version");
                    var t = "BettrDiet Version: " + vers.Value+"\r\n";
                    t += "Plan: " + (_ds.CurrentNutritionPlan!=null ? _ds.CurrentNutritionPlan.Name : "")+"\r\n";
                    t += "Userid: " + (_ds.UserData!= null ? _ds.UserData._id : "") + "\r\n";
                    var response = await WebClientAsyncExtensions.SendFeedbackTask(BettrFitDataSource.Instance.getAuth(), t+"\r\n"+Text);
                    BettrFitDataSource.Instance._sync.EndSync();
                    if (response.Error == null)
                    {
                        this.Close(this);
                    }
                }
                catch (Exception ex)
                {
                    BettrFitDataSource.Instance._sync.EndSync();
                }
            }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        
       
        public bool Sync
        {
            get
            {
                Debug.WriteLine("Syncin:" + _ds._sync.IsSyncing);
                return _ds._sync.IsSyncing;
            }
        }

        private IMvxMessenger _messenger;
        private MvxSubscriptionToken _synctoken;
       

        public MvxCommand SendCommand { get; set; }
    }
}
