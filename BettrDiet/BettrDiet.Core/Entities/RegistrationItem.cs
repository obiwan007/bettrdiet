using System;
using System.Collections.Generic;
using System.Linq;

namespace BettrFitSPA.Viewmodels
{
    public class RegistrationItem
    {

        public string Email { get; set; }

        private string pwd1 = "";
        public string Pwd1
        {
            get { return pwd1; }
            set
            {
                pwd1 = value;
                //OnPropertyChanged("ErrorText");
            }
        }

        private string pwd2 = "";
        public string Pwd2
        {
            get { return pwd2; }
            set
            {
                pwd2 = value;
            }
        }

        public int Mode { get; set; }
        public bool IsMale { get; set; }

        public int Goal { get; set; }

        public int Age { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public int FitnessLevel { get; set; }

        public int WorkoutDays { get; set; }

        public int WorkoutMinutes { get; set; }

        public RegistrationItem()
        {
            FitnessLevel = 5;
            Age = 25;
            Height = 175;
            Weight = 76;
            Goal_Endurance = 3;
            Goal_Muscle = 3;
            Goal_Posture = 3;
            Goal_Strength = 3;
            Goal_FatLoss = 3;
            WorkoutDays = 3;
            WorkoutMinutes = 60;
            IsMale = true;

        }

        //private string _errorText;

        //public string ErrorText
        //{
        //    get
        //    {
        //        if (Pwd1.Length == 0 && Pwd2.Length == 0)
        //        {
        //            IsSomethingWrong = true;
        //            return "";
        //        }
        //        string _errorText = "";
        //        if (Pwd1 != Pwd2)
        //        {
        //            _errorText = CultureHelper.GetLocalString("Your Passwords are not the same!|Die Passworte sind nicht gleich!");
        //        }
        //        else if (Pwd1.Length < 6)
        //        {
        //            _errorText = CultureHelper.GetLocalString("   Your password is to short!|Dein Passwort ist zu kurz");
        //        }
        //        if (Email.Length != 0)
        //        {
        //            if (Email.Length < 8)
        //            {
        //                _errorText = CultureHelper.GetLocalString("   Invalid email address!|Ungültige Email Adresse!");
        //            }
        //            try
        //            {
        //                MailAddress address = new MailAddress(Email);
        //                Log.Debug(this.ToString(), "Mail:" + address.Address + " " + address.DisplayName);
        //                if (!IsValidEmailFormat(Email))
        //                    throw new Exception();
        //            }
        //            catch (Exception ex)
        //            {
        //                _errorText = CultureHelper.GetLocalString("   Something is wrong with your email address!|Es stimmt was mit Deiner Email Adresse nicht!");
        //            }
        //        }
        //        if (_errorText.Length > 0)
        //            IsSomethingWrong = true;
        //        else
        //            IsSomethingWrong = false;
        //        return _errorText;
        //    }
        //    set { _errorText = value; }
        //}

        //private bool IsValidEmailFormat(string s)
        //{
        //    return Regex.IsMatch(s, @"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");
        //}

        //bool _isSomethingWrong = true;
        //public bool IsSomethingWrong
        //{
        //    get
        //    {
        //        var a = ErrorText;
        //        Log.Debug(this.ToString(), "Errortext:" + a);
        //        return _isSomethingWrong;

        //    }
        //    set
        //    {
        //        SetProperty(ref _isSomethingWrong, value, "IsSomethingWrong");
        //    }
        //}


        //public bool Is1Day
        //{
        //    get
        //    {
        //        if (WorkoutDays == 1)
        //            return true;
        //        else
        //            return false;
        //    }
        //    set
        //    {
        //        if (value)
        //            WorkoutDays = 1;
        //    }
        //}
        //public bool Is3Day
        //{
        //    get
        //    {
        //        if (WorkoutDays == 3)
        //            return true;
        //        else
        //            return false;
        //    }
        //    set
        //    {
        //        if (value)
        //            WorkoutDays = 3;
        //    }
        //}

        //public bool Is5Day
        //{
        //    get
        //    {
        //        if (WorkoutDays == 5)
        //            return true;
        //        else
        //            return false;
        //    }
        //    set
        //    {
        //        if (value)
        //            WorkoutDays = 5;
        //    }
        //}

        //public bool Is20Min
        //{
        //    get
        //    {
        //        if (WorkoutMinutes == 20)
        //            return true;
        //        else
        //            return false;
        //    }
        //    set
        //    {
        //        if (value)
        //            WorkoutMinutes = 20;
        //    }
        //}

        //public bool Is40Min
        //{
        //    get
        //    {
        //        if (WorkoutMinutes == 40)
        //            return true;
        //        else
        //            return false;
        //    }
        //    set
        //    {
        //        if (value)
        //            WorkoutMinutes = 40;
        //    }
        //}

        //public bool Is60Min
        //{
        //    get
        //    {
        //        if (WorkoutMinutes == 60)
        //            return true;
        //        else
        //            return false;
        //    }
        //    set
        //    {
        //        if (value)
        //            WorkoutMinutes = 60;
        //    }
        //}

        public int AvailableWorkoutPlanSel { get; set; }

        public int Goal_Posture { get; set; }
        public int WorkoutDaysSel { get; set; }

        private int _goal_FatLoss;
        public int Goal_FatLoss
        {
            get { return _goal_FatLoss; }
            set
            {
                _goal_FatLoss = value;
                calcMax();
            }
        }

        private int _goal_Strength;
        public int Goal_Strength
        {
            get { return _goal_Strength; }
            set
            {
                _goal_Strength = value;
                calcMax();
            }
        }

        private int _goal_Muscle;
        public int Goal_Muscle
        {
            get { return _goal_Muscle; }
            set
            {
                _goal_Muscle = value;
                calcMax();
            }
        }

        private int _goal_Endurance;
        public int Goal_Endurance
        {
            get { return _goal_Endurance; }
            set
            {
                _goal_Endurance = value;
                calcMax();
            }
        }

        private void calcMax()
        {
            var sum = Goal_Endurance + Goal_Muscle + Goal_FatLoss;
            int dif = (15 - sum);
            if (dif >= 0)
                return;
            if (Goal_Endurance > 1)
                Goal_Endurance -= 1;


            if (Goal_FatLoss > 1)
                Goal_FatLoss -= 1;


            if (Goal_Muscle > 0)
                Goal_Muscle -= 1;
        }

        public string Nickname { get; set; }
    }
}