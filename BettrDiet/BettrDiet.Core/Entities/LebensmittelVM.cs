using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BettrFitSPA.Viewmodels
{
    public class LebensmittelVM
    {
        public string _id { get; set; }
        public string _accessId { get; set; }

        public float BE { get; set; }
        public float? Calcium { get; set; }
        public float? Carotin { get; set; }
        public float? Eisen { get; set; }
        public float Fett { get; set; }
        public float? FettGes { get; set; }
        public float? FettUnges { get; set; }
        public float? Fols { get; set; }

        public float? Kalium { get; set; }
        public float kCalGesamt { get; set; }
        public float KH { get; set; }
        public float kJoule { get; set; }
        public string Klasse { get; set; }
        public float? Magnesium { get; set; }
        public string Name { get; set; }
        public float? Natrium { get; set; }
        public float? Omega3 { get; set; }
        public float? Omega6 { get; set; }
        public float? Phosphor { get; set; }
        public float Prot { get; set; }

        public int UsageCount { get; set; }
        public float? VitA { get; set; }
        public float? VitB1 { get; set; }
        public float? VitB2 { get; set; }
        public float? VitB6 { get; set; }
        public float? VitC { get; set; }
        public float? VitE { get; set; }
        public float? Zink { get; set; }
        public bool? IsPrivate { get; set; }
        public string User_id { get; set; }
        public float? Asche { get; set; }
        public float? BetaCarotin { get; set; }
        public float? Cholin { get; set; }
        public float? Fiber { get; set; }
        public string Hersteller { get; set; }
        public float? Kupfer { get; set; }
        public float? Lut_Zea { get; set; }
        public float? Lycopene { get; set; }
        public float? Mangan { get; set; }
        public float? Niacin { get; set; }
        public float? Retinol { get; set; }
        public float? Riboflavin { get; set; }
        public float? Selen { get; set; }
        public float? Thiamin { get; set; }
        public float? VitB12 { get; set; }
        public string DbSource { get; set; }
        public string Lang { get; set; }
        public RezeptVM Rezept { get; set; }
        public bool IsPlan { get; set; }
        //public string Plan { get; set; }
        //public string PlanTag { get; set; }
        //public string MengeForEinheit { get; set; }
        //public string Einheit { get; set; }

        public List<LebensmittelPlanEntryVM> PlanEntries { get; set; }

        public bool IsGlutenFree { get; set; }
        public bool IsVegan { get; set; }
        public bool IsVegetarisch { get; set; }
        public bool IsLactoseFree { get; set; }
        public string Image { get; set; }

        public string CurrentPlanType { get; set; }


    }
}
