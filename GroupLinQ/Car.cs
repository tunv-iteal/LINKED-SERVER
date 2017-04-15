using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupLinQ
{
    public class Car
    {
        public string Vin { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
    }

    public class Repair
    {
        public string Vin { get; set; }
        public string Desc { get; set; }
        public decimal Cost { get; set; }
    }

}