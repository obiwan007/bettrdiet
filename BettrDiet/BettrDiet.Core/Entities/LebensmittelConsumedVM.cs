using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BettrFitSPA.Viewmodels
{
    public class LebensmittelConsumedVM 
    {
        public string _id { get; set; }
        public string _accessId { get; set; }

        public short Mahlzeit { get; set; }
        public float Menge { get; set; }
        public string Name { get; set; }        
        public string Lebensmittel_id { get; set; }
        public string Lebensmittel_Image { get; set; }
        public string User_id { get; set; }

        public string PlanTag { get; set; }
        public string Einheit { get; set; }
        public string MengeProEinheit { get; set; }
        public DateTime Datum { get; set; }


        private LebensmittelVM _lebensmittel;
        public LebensmittelVM Lebensmittel
        {
            get
            {
                return _lebensmittel;
            }
            set
            {
                _lebensmittel = value;
            }
        }

        private double _kCal;
        public double KCal
        {
            get
            {
                if (Lebensmittel != null)
                {
                    _kCal = Math.Round(Lebensmittel.kCalGesamt * Menge / 100.0, 1);
                }
                return _kCal;
            }
            set { _kCal = value; }
        }

        private double _kJoule;
        public double KJoule
        {
            get
            {
                if (Lebensmittel != null)
                {
                    _kJoule = Math.Round(Lebensmittel.kJoule * Menge / 100.0, 1);
                }
                return _kJoule;
            }
            set { _kJoule = value; }
        }

        private double _fat;
        public double Fat
        {
            get
            {
                if (Lebensmittel != null)
                {
                    _fat = Math.Round(Lebensmittel.Fett * Menge / 100.0, 1);
                }
                return _fat;
            }
            set { _fat = value; }
        }

        private double _prot;
        public double Prot
        {
            get
            {
                if (Lebensmittel != null)
                {
                    _prot = Math.Round(Lebensmittel.Prot * Menge / 100.0, 1);
                }
                return _prot;
            }
            set { _prot = value; }
        }

        private double _kh;
        public double KH
        {
            get
            {
                if (Lebensmittel != null)
                {
                    _kh = Math.Round(Lebensmittel.KH * Menge / 100.0, 1);
                }
                return _kh;
            }
            set { _kh = value; }
        }
    }
}
