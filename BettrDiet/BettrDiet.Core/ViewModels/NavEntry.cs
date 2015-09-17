using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BettrDiet.Core.ViewModels
{
    public class NavEntry
    {
        public NavEntry(string name, string img)
        {
            Name = name;
            Img = "/Assets/Images/Dark/"+img+".png";
        }
        public string Name { get; set; }
        public string Img { get; set; }
    }
}
