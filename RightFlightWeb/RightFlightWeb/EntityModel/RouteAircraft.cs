using System;
using System.Collections.Generic;

namespace RightFlightWeb.EntityModel
{
    public partial class RouteAircraft
    {
        public RouteAircraft()
        {
            Flight = new HashSet<Flight>();
        }

        public int RouteAircraftId { get; set; }
        public string IcaoTypeCode { get; set; }
        public int RouteId { get; set; }
        public int FlightDuration { get; set; }

        public virtual Aircraft Aircraft { get; set; }
        public virtual Route Route { get; set; }
        public virtual ICollection<Flight> Flight { get; set; }
    }
}
