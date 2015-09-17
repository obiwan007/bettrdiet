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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Common.Models;
using Cirrious.MvvmCross.WindowsPhone.Views;
using System.ServiceModel;
using BettrDiet.Core.Common;
using BettrFit.Core.Common;
using BettrDiet.Core.ViewModels;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using BettrDiet.Core.Events;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

using Windows.ApplicationModel.Store;
using Microsoft.Advertising.Mobile.UI;
using System.Diagnostics;


namespace BettrDiet.WP8.Views
{
    public partial class MainView : MvxPhonePage
    {
        private MvxSubscriptionToken _alertNotify;
        private MvxSubscriptionToken _toastNotify;
        private MvxSubscriptionToken _showAgb;
        private MvxSubscriptionToken _email;
        private MvxSubscriptionToken _rate;
        private MvxSubscriptionToken _shake;
        // Constructor
        public MainView()
        {
            InitializeComponent();

            //Shows the trial reminder message, according to the settings of the TrialReminder.
            //(App.Current as App).trialReminder.Notify();

            ////Shows the rate reminder message, according to the settings of the RateReminder.
            //(App.Current as App).rateReminder.Notify(); 


            //AdControl adControl = new AdControl("test_client",   // ApplicationID
            //                               "Image480_80",   // AdUnitID
            //                               true);           // isAutoRefreshEnabled
            //// Make the AdControl size large enough that it can contain the image
            //adControl.Width = 480;
            //adControl.Height = 80;

            //adcontrol.ApplicationId = "test_client";
            //adcontrol.AdUnitId = "Image480_80";

            //Grid grid = (Grid)this.LayoutRoot.Children[2];
            //grid.Children.Add(adControl);



            var _messenger = Mvx.Resolve<IMvxMessenger>();
            _alertNotify = _messenger.SubscribeOnMainThread<PopupEvent>((e) =>
            {
                MessageBox.Show(e.Message, e.Header, MessageBoxButton.OK);
            });

            _alertNotify = _messenger.Subscribe<SyncEvent>((e) =>
            {
                
                try
                {
                    if (SystemTray.ProgressIndicator==null)
                    {
                        ProgressIndicator pi = new ProgressIndicator();
                        SystemTray.ProgressIndicator = pi;
                        SystemTray.ProgressIndicator.IsIndeterminate = true;
                    }
                    SystemTray.ProgressIndicator.IsVisible = e.IsSyncing;
                }
                catch(Exception ex)
                {

                }
            });

            _showAgb = _messenger.Subscribe<ShowAGBEvent>((e) =>
            {

                OnShowAGB();
            });

            _shake = _messenger.Subscribe<ShakeEvent>((e) =>
            {
                PurchaseProduct("smallshake");
            });

            _toastNotify = _messenger.Subscribe<KalorienUpdatedEvent>((e) =>
            {
                ShellTile TileToFind = ShellTile.ActiveTiles.First();

                // Application should always be found
                if (TileToFind != null)
                {
                    // set the properties to update for the Application Tile
                    // Empty strings for the text values and URIs will result in the property being cleared.

                    var ds = BettrFitDataSource.Instance;
                    var ud = ds.UserDaily.FirstOrDefault();

                    IconicTileData oIcontile = new IconicTileData();
                    oIcontile.Title = "BettrDiet";
                    oIcontile.Count = 0;

                    oIcontile.IconImage = new Uri("Assets/Images/tileLogoTrans.png", UriKind.Relative);
                    oIcontile.SmallIconImage = new Uri("Assets/Images/tiny.png", UriKind.Relative);

                    oIcontile.WideContent1 = e.Calories+" kCal";
                    oIcontile.WideContent2 = CultureHelper.GetLocalString("Day |Tag ") + (DateTime.Now - ds.CurrentNutritionPlan.StartDate).Days+ "/" + ds.CurrentNutritionPlan.Days.Count.ToString();
                    oIcontile.WideContent3 = CultureHelper.GetLocalString("Your weight today " + ud.Weight + "kg|Dein Gewicht heute " + ud.Weight + "kg");
                    TileToFind.Update(oIcontile);
                }
            });

            _email = _messenger.Subscribe<EmailEvent>((e) =>
            {
                EmailComposeTask emailTask = new EmailComposeTask();
                emailTask.To = "support@bettrfit.com";
                emailTask.Show();
            });

            _rate = _messenger.Subscribe<RateEvent>((e) =>
            {
                MarketplaceReviewTask reviewTask = new MarketplaceReviewTask();
                reviewTask.Show();
            });


              // Application Tile is always the first Tile, even if it is not pinned to Start.
          
            //IconicTileData oIcontile = new IconicTileData();
            //oIcontile.Title = "Hello Iconic Tile!!";
            //oIcontile.Count = 7;

            //oIcontile.IconImage = new Uri("Assets/tiny.png", UriKind.Relative);
            //oIcontile.SmallIconImage = new Uri("Assets/tileLogo", UriKind.Relative);

            //oIcontile.WideContent1 = "Tageskalorien:";
            //oIcontile.WideContent2 = "Tag 12/30";
            //oIcontile.WideContent3 = "Dein Gewicht heute:";

            //oIcontile.BackgroundColor = System.Windows.Media.Colors.Orange;

            //// find the tile object for the application tile that using "Iconic" contains string in it.
            //ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("Iconic".ToString()));

            //foreach (var t in ShellTile.ActiveTiles)
            //{
            //    TileToFind = t;
            //}

            ////TileToFind.Delete();
            //ShellTile.Create(new Uri("/Views/MainPage.xaml?id=Iconic", UriKind.Relative), oIcontile, true);

            //if (TileToFind != null && TileToFind.NavigationUri.ToString().Contains("Iconic"))
            //{
            //    //TileToFind.Delete();
            //    //ShellTile.Create(new Uri("/Views/MainPage.xaml?id=Iconic", UriKind.Relative), oIcontile, true);
            //}
            //else
            //{
            //    //ShellTile.Create(new Uri("/Views/MainPage.xaml?id=Iconic", UriKind.Relative), oIcontile, true);
            //}

            //FlipTileData TileData = new FlipTileData()
            //{
            //    Title = "[title]",
            //    BackTitle = "[back of Tile title]",
            //    BackContent = "[back of medium Tile size content]",
            //    WideBackContent = "[back of wide Tile size content]",
            //    Count = 0,
            //};
            //ShellTile.Create(null,TileData);

            //ShellTileSchedule SampleTileSchedule = new ShellTileSchedule(t);
            //bool TileScheduleRunning = false;


            //// Update will happen one time.
            //SampleTileSchedule.Recurrence = UpdateRecurrence.Onetime;

            //// Start the update schedule now. 
            //SampleTileSchedule.StartTime = DateTime.Now;
            
            ////SampleTileSchedule.RemoteImageUri = new Uri(@"http://www.weather.gov/forecasts/graphical/images/conus/MaxT1_conus.png");
            //SampleTileSchedule.Start();
            //TileScheduleRunning = true;


        }

        async void PurchaseProduct(string productId)
        {
            try
            {
                // Kick off purchase; don't ask for a receipt when it returns
                await CurrentApp.RequestProductPurchaseAsync(productId, false);

                MessageBox.Show("Danke für die Einladung!\r Ich werde den Shake genießen!", "Vielen Dank!", MessageBoxButton.OK);

            }
            catch (Exception ex)
            {
                // When the user does not complete the purchase (e.g. cancels or navigates back from the Purchase Page), an exception with an HRESULT of E_FAIL is expected.
            }
        }


        private static async System.Threading.Tasks.Task OnShowAGB()
        {
            try
            {
                var c = CultureHelper.GetCurrentCulture().Substring(0, 2);
                var uri = new Uri("ms-appx:///Assets/AGB_" + c + ".html");
                var options = new Windows.System.LauncherOptions();
                options.DisplayApplicationPicker = false;
                //nav.Navigate(uri);
                var f = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);

                Windows.System.Launcher.LaunchFileAsync(f);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Navigates to about page.
        /// </summary>
        private void GoToAbout(object sender, Telerik.Windows.Controls.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/About.xaml", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Navigates to template page.
        /// </summary>
        private void GoToHowTo(object sender, Telerik.Windows.Controls.GestureEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/HowTo.xaml", UriKind.RelativeOrAbsolute));
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            listMenu.SelectedIndex = -1;
        }


        private void refreshIconButton_Click(object sender, EventArgs e)
        {
            (DataContext as MainViewModel).RefreshCommand.Execute();

        }

        private void Info_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/About.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            (DataContext as MainViewModel).LogoffCommand.Execute();
        }

        private void AdControl_AdRefreshed(object sender, EventArgs e)
        {
            Debug.WriteLine("Ad Refreshed");
        }

        private void adcontrol_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
            Debug.WriteLine("Ad Error"+e.Error+" "+e.ErrorCode);
        }
    }
}
