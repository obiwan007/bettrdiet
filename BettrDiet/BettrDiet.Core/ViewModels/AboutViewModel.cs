// --------------------------------------------------------------------------------------------------------------------
// <summary>
//    Defines the FirstViewModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace BettrDiet.Core.ViewModels
{
    using System.Windows.Input;
    using Cirrious.MvvmCross.ViewModels;
    using System.ServiceModel;
    using Cirrious.MvvmCross.Plugins.Messenger;
    using Cirrious.CrossCore;
    using BettrDiet.Core.Events;

    /// <summary>
    /// Define the FirstViewModel type.
    /// </summary>
    public class AboutViewModel : BaseViewModel
    {
             public ICommand RateThisAppCommand
        {
            get;
            private set;
        }

        public ICommand SendAnEmailCommand
        {
            get;
            private set;
        }

        public AboutViewModel()
        {
            RateThisAppCommand = new MvxCommand(OnRateThisApp);
            SendAnEmailCommand = new MvxCommand(OnEmail);
        }

        private void OnEmail()
        {
            Mvx.Resolve<IMvxMessenger>().Publish<EmailEvent>(new EmailEvent(this));

        }

        private void OnRateThisApp()
        {
            Mvx.Resolve<IMvxMessenger>().Publish<RateEvent>(new RateEvent(this));
        }
        
    }
}
