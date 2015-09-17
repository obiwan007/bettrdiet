using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using Cirrious.MvvmCross.ViewModels;
using BettrFitSPA.Viewmodels.User;
using BettrFit.Core.Common;
using BettrDiet.Core.Common;
using Cirrious.CrossCore;
using Chance.MvvmCross.Plugins.UserInteraction;
using Cirrious.MvvmCross.Plugins.Messenger;
using BettrDiet.Core.Events;

namespace BettrDiet.Core.ViewModels
{

    public class UserDataVM : BaseViewModel
    {

        public UserDataVM()
        {

        }

        private string _email = "";
        public string Email
        {
            get { return _email; }
            set
            {
                SetProperty(ref _email, value, ()=>Email);
                if (!(_email.Contains("@") && _email.Contains(".")))
                {

                    throw new ArgumentException(CultureHelper.GetLocalString("Email ID is Invalid|Email ist nicht gültig!"));

                }

            }
        }

        private string _pwd1;
        public string Pwd1
        {
            get { return _pwd1; }
            set
            {
                SetProperty(ref _pwd1, value, ()=>Pwd1);

                if (value.Length < 7)
                {
                    throw new ArgumentException(CultureHelper.GetLocalString("Password must be more than 6 charakters!|Passwort muss mehr wie 6 Zeichen haben!"));
                }
                if (value != _pwd2 && !string.IsNullOrEmpty(_pwd2))
                {
                    throw new ArgumentException(CultureHelper.GetLocalString("Password 1 must be same as password 2|Das 1. Passwort muss dem 2. gleichen!"));
                }
            }
        }


        private string _pwd2;
        //[Compare("_pwd1", ErrorMessage = "Passwords do not match")]
        public string Pwd2
        {
            get { return _pwd2; }
            set
            {
                SetProperty(ref _pwd2, value, ()=>Pwd2);
                //if (value.Length < 7)
                //{
                //    throw new ArgumentException(CultureHelper.GetLocalString("Password must be more than 6 charakters!|Passwort muss mehr wie 6 Zeichen haben!"));
                //}
                if (value != _pwd1)
                {
                    throw new ArgumentException(CultureHelper.GetLocalString("Password 2 must be same as password 1|Das 2. Passwort muss dem 1. gleichen!"));
                }
            }
        }

        private string _nickname;
        public string Nickname
        {
            get { return _nickname; }
            set
            {
                SetProperty(ref _nickname, value, ()=>Nickname);
            }
        }
    }


    public class RegisterViewModel : BaseViewModel
    {
        private BettrFitDataSource _ds;
        private SyncDataViewModel _sync;
        private bool _canSave = true;

        public MvxCommand GoBackCommand { get; set; }
        public MvxCommand RegisterCommand { get; set; }


        Dictionary<int, int> mapDays = new Dictionary<int, int> { { 0, 1 }, { 1, 3 }, { 2, 5 } };
        Dictionary<int, int> mapMinutes = new Dictionary<int, int> { { 0, 30 }, { 1, 45 }, { 2, 60 } };

        public RegisterViewModel()
        {
            _ds = BettrFitDataSource.Instance;
            _sync = BettrFitDataSource.Instance._sync;

            RegisterCommand = new MvxCommand(()=>registerNewUser());

            Selected = new UserGoalVM();
            Selected.Goal_Muscle = 8;
            Selected.Goal_FatLoss = 5;
            Selected.Goal_Endurance = 2;
            Selected.WorkoutDays = 3;
            Selected.WorkoutMinutes = 45;
            _goal_Endurance = _selected.Goal_Endurance;
            _goal_FatLoss = _selected.Goal_FatLoss;
            _goal_Muscle = _selected.Goal_Muscle;
            OnPropertyChanged("Goal_Muscle");
            OnPropertyChanged("Goal_Endurance");
            OnPropertyChanged("Goal_FatLoss"); ;

            _userdata.PropertyChanged += (a, b) =>
            {                
                RaisePropertyChanged("IsValid");
            };
            _messenger = Mvx.Resolve<IMvxMessenger>();
        }

        public bool IsValid
        {
            get
            {
                if (AgbAccepted == false)
                    return false;

                if (string.IsNullOrEmpty(User.Email))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(User.Pwd1))
                {
                    return false;
                }
                if (User.Pwd1 != User.Pwd2)
                {
                    return false;
                }
                if (User.Pwd1.Length < 6)
                {
                    return false;
                }
                return true;
            }
        }

        private async void registerNewUser()
        {
            if (string.IsNullOrEmpty(User.Email))
            {
                return;
            }
            if (string.IsNullOrEmpty(User.Pwd1))
            {
                return;
            }
            if (User.Pwd1 != User.Pwd2)
            {
                return;
            }
            if (User.Pwd1.Length < 6)
            {
                return;
            }

            var vm = new WebAccess.ServiceReference.RegistrationItem();
            vm.Age = Age;
            vm.Email = User.Email;
            vm.Goal_Endurance = (int)Goal_Endurance;
            vm.Goal_FatLoss = (int)Goal_FatLoss;
            vm.Goal_Muscle = (int)Goal_Muscle;
            vm.Height = (int)Height;
            vm.IsMale = _isMale;
            vm.Nickname = User.Nickname;
            vm.Pwd1 = User.Pwd1;
            vm.Pwd2 = User.Pwd2;
            vm.Weight = (int)Weight;
            vm.WorkoutDays = (int)_selected.WorkoutDays;
            vm.WorkoutMinutes = (int)_selected.WorkoutMinutes;


            var ret = await _ds.RegisterNewUser(vm);


            if (ret == 0)
            {
                //CurrentFrame.NavigationService.GoBack();
                this.Close(this);
                return;
            }

            var f = "";// CultureHelper.GetLocalString("An Error occurred.  Please check email address. Maybe its already used.|Ein Fehler ist aufgetreten. Bitte prüfe die email Adresse. Sie ist wahrscheinlich bereits in Benutzung.");
            if (ret == -3)
            {
                f = CultureHelper.GetLocalString("An Error occurred.  Please check email address. Maybe its already used.|Ein Fehler ist aufgetreten. Bitte prüfe die email Adresse. Sie ist wahrscheinlich bereits in Benutzung.");
            }
            else if (ret == -4)
            {
                f = CultureHelper.GetLocalString("An Error occurred.  Please change nickname. Maybe its already used.|Ein Fehler ist aufgetreten. Bitte prüfe deinen Nicknamen. Er ist wahrscheinlich bereits in Benutzung.");
            }
            else if (ret != 0)
                f = CultureHelper.GetLocalString("An Error occurred.");

            _messenger.Publish(new PopupEvent(this, f,"Fehler"));

            
            //MessageBox.Show(f,
            //   "Error", MessageBoxButton.OK);
        }

        public SyncDataViewModel Sync
        {
            get
            {
                return _sync;
            }
        }

        UserGoalVM _selected = null;
        public UserGoalVM Selected
        {
            get
            {
                if (_selected == null)
                    Debug.WriteLine("Nothing is active");
                return _selected;
            }
            set
            {
                _selected = (UserGoalVM)value;
                OnPropertyChanged("Selected");
                OnPropertyChanged("SelectedDays");
                _goal_Endurance = 0;
                _goal_FatLoss = 0;
                _goal_Muscle = 0;
                _goal_Endurance = _selected.Goal_Endurance;
                _goal_FatLoss = _selected.Goal_FatLoss;
                _goal_Muscle = _selected.Goal_Muscle;
                OnPropertyChanged("Goal_Muscle");
                OnPropertyChanged("Goal_Endurance");
                OnPropertyChanged("Goal_FatLoss");
            }
        }

        private int _selectedDays = 0;
        public int SelectedDays
        {
            get
            {
                if (_selected == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return -1;
                }

                return mapDays.FirstOrDefault(a => a.Value == _selected.WorkoutDays).Key;


            }
            set
            {
                if (_selected == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return;
                }
                _selected.WorkoutDays = (short)mapDays[value];

                SetProperty(ref _selectedDays, value, ()=>SelectedDays);
            }
        }

        public int SelectedMinutes
        {
            get
            {
                if (_selected == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return -1;
                }

                return mapMinutes.FirstOrDefault(a => a.Value == _selected.WorkoutMinutes).Key;


            }
            set
            {
                if (_selected == null)
                {
                    Debug.WriteLine("Nothing is active");
                    return;
                }
                _selected.WorkoutMinutes = (short)mapMinutes[value];

                SetProperty(ref _selectedDays, value, ()=>SelectedMinutes);
            }
        }

        private double _goal_Endurance;
        public double Goal_Endurance
        {
            get { return _goal_Endurance; }
            set
            {
                SetProperty(ref _goal_Endurance, value,()=>Goal_Endurance);
                calcMax();
                if (_selected != null)
                    _selected.Goal_Endurance = (byte)_goal_Endurance;
            }
        }
        private double _goal_Muscle;
        public double Goal_Muscle
        {
            get { return _goal_Muscle; }
            set
            {
                SetProperty(ref _goal_Muscle, value,()=>Goal_Muscle);
                calcMax();
                if (_selected != null)
                    _selected.Goal_Muscle = (byte)_goal_Muscle;
            }
        }
        private double _goal_FatLoss;
        public double Goal_FatLoss
        {
            get { return _goal_FatLoss; }
            set
            {
                SetProperty(ref _goal_FatLoss, value,()=>Goal_FatLoss);
                calcMax();
                if (_selected != null)
                    _selected.Goal_FatLoss = (byte)_goal_FatLoss;

            }
        }

        private void calcMax()
        {
            var sum = Goal_Endurance + Goal_Muscle + Goal_FatLoss;
            int dif = (int)(15 - sum);
            if (dif >= 0)
                return;
            if (Goal_Endurance > 1)
                Goal_Endurance -= 1;


            if (Goal_FatLoss > 1)
                Goal_FatLoss -= 1;


            if (Goal_Muscle > 0)
                Goal_Muscle -= 1;
        }

        private UserDataVM _userdata = new UserDataVM();

        public UserDataVM User
        {
            get { return _userdata; }
            set { _userdata = value; }
        }

        private int _weight = 75;
        public int Weight
        {
            get { return _weight; }
            set
            {
                SetProperty(ref _weight, value,()=>Weight);
            }
        }

        private int _height = 180;
        public int Height
        {
            get { return _height; }
            set
            {
                SetProperty(ref _height, value,()=>Height);
            }
        }

        private int _age = 25;
        public int Age
        {
            get { return _age; }
            set
            {
                SetProperty(ref _age, value,()=>Age);
            }
        }

        private bool _isMale = true;
        public int MaleFemale
        {
            get { return _isMale == true ? 0 : 1; }
            set
            {
                _isMale = value == 0 ? true : false;
            }
        }

        private bool _agbAccepted = false;
        private IMvxMessenger _messenger;
        public bool AgbAccepted
        {
            get { return _agbAccepted; }
            set
            {
                SetProperty(ref _agbAccepted, value,()=>AgbAccepted);                
                RaisePropertyChanged("IsValid");
            }
        }
    }
}
