using BettrDiet.Windows8.Common;
using BettrFit.Core.Common;
using Cirrious.MvvmCross.WindowsStore.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Standardseite" ist unter http://go.microsoft.com/fwlink/?LinkId=234237 dokumentiert.

namespace BettrDiet.Windows8.Views
{
    /// <summary>
    /// Eine Standardseite mit Eigenschaften, die die meisten Anwendungen aufweisen.
    /// </summary>
    public sealed partial class Login : MvxStorePage
    {        
        public Login()
        {
            this.InitializeComponent();
            //this.navigationHelper = new NavigationHelper(this);
            //this.navigationHelper.LoadState += navigationHelper_LoadState;
            //this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.Loaded += Login_Loaded;
        }

        async void Login_Loaded(object sender, RoutedEventArgs e)
        {
            ShowLogin();
        }

        private async void ShowLogin()
        {
            CredentialPickerOptions opt = new CredentialPickerOptions();
            opt.AuthenticationProtocol = AuthenticationProtocol.Basic;
            opt.Message = CultureHelper.GetLocalString("Login to BettrDiet with your email adress|Mit deiner email Adresse bei BettrDiet anmelden");
            opt.TargetName = ".";
            opt.Caption = "BettrDiet Login";
            opt.CredentialSaveOption = CredentialSaveOption.Selected;
            
            var vault = new Windows.Security.Credentials.PasswordVault();

            var vm = DataContext as BettrDiet.Core.ViewModels.LoginViewModel;
            
            var credentialPicker = await Windows.Security.Credentials.UI.CredentialPicker.PickAsync(opt);

            if (credentialPicker.ErrorCode!=0)
            {
                
                vm.DoClose();
            }

            if (credentialPicker.CredentialUserName != null && credentialPicker.CredentialPassword != null)
            {

                if (credentialPicker.CredentialSaveOption == CredentialSaveOption.Selected)
                {

                    var s = credentialPicker.Credential;

                    var str = System.Runtime.InteropServices.WindowsRuntime.WindowsRuntimeBufferExtensions.AsStream(s);
                    byte[] bytes = new byte[100];
                    var sr = new System.IO.StreamReader(str);
                    while (!sr.EndOfStream)
                    {
                        var line = await sr.ReadLineAsync();
                        Debug.WriteLine("line:" + line);
                    }

                    vm.Username = credentialPicker.CredentialUserName;
                    vm.Pwd = credentialPicker.CredentialPassword;
                    var ret=await vm.DoLogin();
                    if (!string.IsNullOrEmpty(ret))
                    {
                        ShowLogin();
                    }
                }



            }
        }        
    }
}
