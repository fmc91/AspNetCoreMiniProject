using System;
using System.Collections.Generic;

namespace RightFlightWeb.Models
{
    public class ClassPricingScheme
    {
        public string TravelClassCode { get; set; }

        public string TravelClassName { get; set; }

        public float AdultFare { get; set; }

        public float ChildFare { get; set; }

        public float InfantFare { get; set; }
    }
}
