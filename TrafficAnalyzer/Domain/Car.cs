using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TrafficAnalyzer.Enumerators;

namespace TrafficAnalyzer.Domain
{
    public class Car : IEqualityComparer<Car>
    {
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public Color ColorEnum { get; set; }
        public DateTime ActivationDate { get; set; }
        public string CarMake { get; set; }

        public bool Equals([AllowNull] Car x, [AllowNull] Car y)
        {
            return x.LicensePlate.Equals(y.LicensePlate);
        }

        public int GetHashCode([DisallowNull] Car obj)
        {
            return HashCode.Combine(obj.LicensePlate);
        }
    }
}
