using System;
using System.Collections.Generic;

namespace RightFlightWeb.Data
{
    public partial class Aircraft
    {
        public Aircraft()
        {
            RouteAircraft = new HashSet<RouteAircraft>();
        }

        public string IcaoTypeCode { get; set; }
        public string Model { get; set; }

        public virtual ICollection<RouteAircraft> RouteAircraft { get; set; }
    }
}
