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

    /// <summary>
    /// Define the FirstViewModel type.
    /// </summary>
    public class FirstViewModel : BaseViewModel
    {

        public FirstViewModel()
        {
            var bind = new BasicHttpBinding();
            bind.Security.Mode = BasicHttpSecurityMode.Transport;

            bind.MaxReceivedMessageSize = 1024 * 1024 * 50;
            bind.MaxBufferSize = 1024 * 1024 * 50;
            var addr = new EndpointAddress("https://www.bettrfit.com/WebService.asmx");
            //var client = new WebAccess.ServiceReference.WebServiceSoapClient(bind, addr);

            //client.GetContentCompleted += (a, b) =>
            //{
            //    var aaa = b.Error;
            //};
            //client.GetContentAsync("optimize", "de");
        }

        /// <summary>
        /// Backing field for my property.
        /// </summary>
        private string myProperty = "Mvx Ninja Coder!";

        /// <summary>
        ///  Backing field for my command.
        /// </summary>
        private MvxCommand myCommand;

        /// <summary>
        /// Gets or sets my property.
        /// </summary>
        public string Hello
        {
            get { return this.myProperty; }
            set { this.SetProperty(ref this.myProperty, value, () => this.Hello); }
        }

        /// <summary>
        /// Gets My Command.
        /// <para>
        /// An example of a command and how to navigate to another view model
        /// Note the ViewModel inside of ShowViewModel needs to change!
        /// </para>
        /// </summary>
        public ICommand MyCommand
        {
            get { return this.myCommand ?? (this.myCommand = new MvxCommand(this.Show)); }
        }

        /// <summary>
        /// Show the view model.
        /// </summary>
        public void Show()
        {
            this.ShowViewModel<FirstViewModel>();
        }
    }
}
