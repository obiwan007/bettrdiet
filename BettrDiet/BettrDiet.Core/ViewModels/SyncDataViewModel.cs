using BettrDiet.Core.Events;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Plugins.Messenger;
using System.Windows;


namespace BettrDiet.Core.ViewModels
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SyncDataViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the SyncDataViewModel class.
        /// </summary>
        public SyncDataViewModel()
        {
            _messenger = Mvx.Resolve<IMvxMessenger>();

            _log = Mvx.Resolve<IMvxTrace>();
            sync = 0;
        }

        public void BeginSync()
        {
            sync++;
            IsSyncing = true;
            
        }

        public void EndSync()
        {
            sync--;
            if (sync == 0)
            {
                IsSyncing = false;
                _messenger.Publish<SyncEvent>(new SyncEvent(this,false));
            }
        }


        private bool _isSyncing;
        private IMvxMessenger _messenger;
        private IMvxTrace _log;

        public bool IsSyncing
        {
            get { return _isSyncing; }
            set {
                
                if(this.SetProperty(ref _isSyncing, value,()=>IsSyncing))
                    _messenger.Publish<SyncEvent>(new SyncEvent(this,true));
            _log.Trace(MvxTraceLevel.Diagnostic, "Sync", "IsSyncing:" + _isSyncing);
                
                //if (value == true)
            //    Visible = Visibility.Visible;
            //else
            //    Visible = Visibility.Collapsed;
            
            }
        }

        //private Visibility _visibility=Visibility.Collapsed;

        //public Visibility Visible
        //{
        //    get { return _visibility; }
        //    set { this.SetProperty(ref _visibility, value); }
        //}
        



        public int sync { get; set; }
    }
}