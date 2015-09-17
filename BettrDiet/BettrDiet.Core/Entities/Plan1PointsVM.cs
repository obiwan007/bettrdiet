using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BettrFitSPA.Viewmodels
{

    public class DayTimesVM
    {
       
        public int DayTime { get; set; }

        public float KCal_Fette { get; set; }
        public float KCal_Proteine { get; set; }
        public float KCal_Kohlenhydrate { get; set; }

        public string Comment { get; set; }
        public string Description { get; set; }

        public float SumProt
        {
            get { return (float)Math.Round(KCal_Proteine / 4); }
        }

        public float SumFat
        {
            get { return (float)Math.Round(KCal_Fette / 9); }
        }

        public float SumKH
        {
            get { return (float)Math.Round(KCal_Kohlenhydrate / 4); }
        }

        public int SummeKCal
        {
            get { return (int)( KCal_Fette + KCal_Proteine + KCal_Kohlenhydrate); }
        }
    }

    public class Plan1PointsVM
    {
        public Plan1PointsVM()
        {
            Daytimes = new List<DayTimesVM>();
        }

        public float Fette { get; set; }
        public float Proteine { get; set; }
        public float Milchprodukte { get; set; }
        public float Kohlenhydrate { get; set; }
        public float Obst { get; set; }
        public float Gemuese { get; set; }
        public float Snacks { get; set; }
        public float Gewuerze { get; set; }
        public float SummeKCal { get; set; }

        public float KCal_Fette { get; set; }
        public float KCal_Proteine { get; set; }
        public float KCal_Kohlenhydrate { get; set; }

        public int CalcSummen()
        {
            SummeKCal = KCal_Fette + KCal_Proteine + KCal_Kohlenhydrate;
            return (int)SummeKCal;
            //if (plantype=="Plan3")
            //{
            //    var sum = Fette * 120 + Proteine * 100 + Milchprodukte * 120 + Kohlenhydrate * 50 + Obst * 30 + Gemuese * 20 + Snacks * 100 + Gewuerze * 50;
            //    SummeKCal = KCal_Fette + KCal_Proteine + KCal_Kohlenhydrate;
            //    return (int)sum;

            //}
            //else
            //{
            //    var sum = Fette * 120 + Proteine * 100 + Milchprodukte * 120 + Kohlenhydrate * 100 + Obst * 100 + Gemuese * 20 + Snacks * 100 + Gewuerze * 50;
            //    SummeKCal = sum;
            //    return (int)sum;
            //}
        }

        public float SumProt
        {
            get { return (float)Math.Round(KCal_Proteine/4); }
        }

        public float SumFat
        {
            get { return (float)Math.Round(KCal_Fette/ 9); }
        }

        public float SumKH
        {
            get { return (float)Math.Round(KCal_Kohlenhydrate / 4); }
        }

        public int Level { get; set; }

        public int Stufe { get; set; }

        public int Activity { get; set; }

        public int BMR { get; set; }

        public List<DayTimesVM> Daytimes { get; set; }

    }
}
