using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Telerik.Windows.Controls;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using BettrDiet.Core.Events;
using Microsoft.Phone.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;

namespace BettrDiet.WP8
{
    public partial class App : Application
    {
        /// <summary>
        /// Component used to handle unhandle exceptions, to collect runtime info and to send email to developer.
        /// </summary>
		public RadDiagnostics diagnostics;
        /// <summary>
        /// Component used to remind end users about the trial state of the application.
        /// </summary>
		public RadTrialApplicationReminder trialReminder;

        /// <summary>
        /// Component used to raise a notification to the end users to rate the application on the marketplace.
        /// </summary>
        public RadRateApplicationReminder rateReminder;

        private Task checkNetworkTask;


		/// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            ThemeManager.ToLightTheme();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
               // Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are being GPU accelerated with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

				// Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

			//Creates an instance of the Diagnostics component.
            diagnostics = new RadDiagnostics();

            //Defines the default email where the diagnostics info will be send.
            diagnostics.EmailTo = "support@bettrfit.com";
            diagnostics.IncludeScreenshot = true;
            diagnostics.ApplicationName = "BettrDiet";
            XDocument doc = XDocument.Load("WMAppManifest.xml");
            var app = doc.Root.Element("App");
            var vers = app.Attribute("Version");            
            diagnostics.ApplicationVersion = vers.Value;
            //Initializes this instance.
            diagnostics.Init();
		    //Creates an instance of the RadTrialApplicationReminder component.
            trialReminder = new RadTrialApplicationReminder();

            //Sets the lenght of the trial period.
            trialReminder.AllowedTrialUsageCount = 30;

            //Sets how often the trial reminder is displayed.
            trialReminder.OccurrenceUsageCount = 2;

            //The reminder is shown only if the application is in trial mode. When this property is set to true the application will simulate that it is in trial mode.
            trialReminder.SimulateTrialForTests = true;
        
		      //Creates a new instance of the RadRateApplicationReminder component.
            rateReminder = new RadRateApplicationReminder();

            //Sets how often the rate reminder is displayed.
            rateReminder.RecurrencePerUsageCount = 2;

            var setup = new Setup(RootFrame);
            setup.Initialize();

            DeviceNetworkInformation.NetworkAvailabilityChanged += DeviceNetworkInformation_NetworkAvailabilityChanged;

            _messenger = Mvx.Resolve<IMvxMessenger>();
        }

        void DeviceNetworkInformation_NetworkAvailabilityChanged(object sender, NetworkNotificationEventArgs e)
        {
            Debug.WriteLine("NetworkAvailable Changed " +e.NotificationType+" "+e.NetworkInterface.InterfaceName+" "+e.NetworkInterface.InterfaceState);
        }


        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            //Before using any of the ApplicationBuildingBlocks, this class should be initialized with the version of the application.
            ApplicationUsageHelper.Init("1.0");
            RootFrame.Navigating += RootFrameOnNavigating;

            checkNetworkTask = Task.Factory.StartNew(()=>checkNetwork());
        }

        public bool ScanInitialisation_Connect()
        {
            bool ControlConnexion = false;

            Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType NewReseau;
            NewReseau = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType;

            Debug.WriteLine("Networktype:" + NewReseau);

            switch (NewReseau)
            {


                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.Wireless80211://Wireless Wifi 802.11   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.Ethernet://Ethernet IEEE 802.3   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.Ethernet3Megabit://Ethernet 3mb/s IETF RFC 895   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.FastEthernetFx://Fast Ethernet UTP Fibre Optique  100mb/s 100-   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.FastEthernetT://Fast Ethernet STP Fibre Optique 100mb/s 100-   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.Fddi://Norme Fibre Optique FFDI   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.GenericModem://Modem   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.GigabitEthernet://Gigabite Ethernet 1000 mb/s || 1gb/s   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.HighPerformanceSerialBus://Interface bus serie Haute    
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.IPOverAtm://Interface I.P transfert Asynchrone (ATM)   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.Isdn:// ISDN X.25 X.25   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.Ppp:// Point-to-Point protocole   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.PrimaryIsdn:// Primary Isdn   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.RateAdaptDsl:// RADSL   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.Slip:// Slip IETF RFC 1055   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.TokenRing:// Token RIng 802.5   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.Tunnel:// Tunnel   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.VeryHighSpeedDsl:// VDSL   
                    ControlConnexion = true;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.MobileBroadbandCdma:// VDSL   
                    ControlConnexion = true;

                    break;


                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.MobileBroadbandGsm:// VDSL   
                    ControlConnexion = true;

                    break;


                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.Unknown://Reseautrouvé inconnu   
                    ControlConnexion = false;

                    break;

                case Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None:// Aucun Reseau Trouvé   
                    ControlConnexion = false;

                    break;

            }

            return ControlConnexion;
        }  


        private void checkNetwork()
        {
            var dt = DateTime.Now;
            var r = new Random();
            var num = r.Next(10000);
            exiting = false;
            var isAvailable=false;
            while(exiting==false)
            {
                if (_messenger.HasSubscriptionsFor<NetworkEvent>())
                {
                    isAvailable = NetworkInterface.GetIsNetworkAvailable();

                    //if ((DateTime.Now - dt).TotalSeconds > 24 && (DateTime.Now - dt).TotalSeconds<35)
                    //    isAvailable = false;

                    Debug.WriteLine("NetworkAvailable=" + isAvailable);
                    _messenger.Publish<NetworkEvent>(new NetworkEvent(this, isAvailable));
                }
                Task.Delay(1000).Wait();
            }
        }

        private void RootFrameOnNavigating(object sender, NavigatingCancelEventArgs args)
        {

            args.Cancel = true;
            RootFrame.Navigating -= RootFrameOnNavigating;
            RootFrame.Dispatcher.BeginInvoke(() => { Cirrious.CrossCore.Mvx.Resolve<Cirrious.MvvmCross.ViewModels.IMvxAppStart>().Start(); });
        }


        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (!e.IsApplicationInstancePreserved)
            {
                //This will ensure that the ApplicationUsageHelper is initialized again if the application has been in Tombstoned state.
                ApplicationUsageHelper.OnApplicationActivated();
            }

            //checkNetworkTask = Task.Factory.StartNew(() => checkNetwork());
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            //exiting = true;
			// Ensure that required application state is persisted here.
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            exiting = true;
        }

        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;
        private IMvxMessenger _messenger;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new RadPhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;

            
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

        public bool exiting { get; set; }
    }
}
