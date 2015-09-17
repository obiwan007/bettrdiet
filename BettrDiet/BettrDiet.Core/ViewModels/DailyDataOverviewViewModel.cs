using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Cirrious.MvvmCross.ViewModels;
using BettrFitSPA.Viewmodels.User;
using BettrDiet.Core.Common;

namespace BettrDiet.Core.ViewModels
{
    public class DailyDataOverviewViewModel : BaseViewModel
    {

        BettrFitDataSource ds;

        public DailyDataOverviewViewModel()
        {
            //this.DefaultViewModel["Lebensmittel"] = ds.CurrentLebensmittel;
            //this.DefaultViewModel["Items"] = ds.LebensmittelConsumed;
            //this.DefaultViewModel["SelectedConsumed"] = ds.SelectedConsumed;
            //this.DefaultViewModel["Search"] = ds.LebensmittelResult;
            //this.DefaultViewModel["CurrentLebensmittel"] = ds.CurrentLebensmittel;
            //this.DefaultViewModel["SummaryConsumedDay"] = ds.SummaryConsumedDay;
            //this.DefaultViewModel["SummaryConsumedDaytime"] = ds.SummaryConsumedDaytime;            

            ds = BettrFitDataSource.Instance;

            ds.FillDailyData();
            //SelectedConsumed = null;


            AddDailyCommand = new MvxCommand(AddDaily, canAddDaily);
            DeleteDailyCommand = new MvxCommand(DeleteDaily, canDeleteDaily);
            EditDailyCommand = new MvxCommand(EditDaily, ()=> CanEditDaily);

            MaxDate = DateTime.Today;
            MinDate = MaxDate - TimeSpan.FromDays(12 * 30);

        }
   
        private bool canAddDaily()
        {
                return true;
        }

        private void AddDaily()
        {
            SelectedDailyData = null;
            //ds.CurrentDailyData = null;
            this.ShowViewModel<DailyDataEditViewModel>();
        }

        private bool canDeleteDaily()
        {
            if (SelectedDailyData != null)
                return true;
            else
                return false;
        }

        private void DeleteDaily()
        {
            ds.DeleteDailyData(SelectedDailyData);
        }

        public bool CanEditDaily
        {
            get{
                if (SelectedDailyData != null)
                return true;
            
            else
                return false;
            }
        }

        private void EditDaily()
        {
            this.ShowViewModel<DailyDataEditViewModel>();
        }


        private DateTime _minDate;
        public DateTime MinDate
        {
            get { return _minDate; }
            set { _minDate = value;
            RaisePropertyChanged("MinDate");
            }
        }

        private DateTime _maxDate;
        public DateTime MaxDate
        {
            get { return _maxDate; }
            set
            {
                _maxDate = value;
                RaisePropertyChanged("MaxDate");
            }
        }



        private UserDailyVM _selectedDailyData;

        public UserDailyVM SelectedDailyData
        {
            get { 
                
                return _selectedDailyData; 
            }
            set {
                this.SetProperty(ref _selectedDailyData,value,()=>SelectedDailyData);
                ds.CurrentDailyData = _selectedDailyData;
                DeleteDailyCommand.RaiseCanExecuteChanged();
                EditDailyCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("CanEditDaily");
                Debug.WriteLine("SelectedDailyData");
            }
        }


        public ObservableCollection<UserDailyVM> DailyData
        {
            get { 
                return ds.UserDaily; 
            }            
        }
        
        #region Commands

        public MvxCommand AddDailyCommand
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
        

    }
}
