using System;
using System.Collections.Generic;

namespace RightFlightWeb.EntityModel
{
    public partial class Nationality
    {
        public Nationality()
        {
            Passenger = new HashSet<Passenger>();
        }

        public int NationalityId { get; set; }
        public string CountryName { get; set; }

        public virtual ICollection<Passenger> Passenger { get; set; }
    }
}
