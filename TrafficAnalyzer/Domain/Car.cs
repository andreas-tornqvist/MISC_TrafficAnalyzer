using System;
using System.Collections.Generic;
using System.Text;
using TrafficAnalyzer.Enumerators;

namespace TrafficAnalyzer.Domain
{
    public class Car
    {
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public Color ColorEnum { get; set; }
        public DateTime ActivationDate { get; set; }
        public string CarMake { get; set; }
    }
}
