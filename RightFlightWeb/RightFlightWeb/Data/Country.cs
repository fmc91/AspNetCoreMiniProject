using System;
using System.Collections.Generic;

namespace RightFlightWeb.Data
{
    public partial class Country
    {
        public Country()
        {
            City = new HashSet<City>();
        }

        public string IsoCountryCode { get; set; }
        public string Name { get; set; }

        public virtual ICollection<City> City { get; set; }
    }
}
