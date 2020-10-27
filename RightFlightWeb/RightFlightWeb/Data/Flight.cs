using System;
using System.Collections.Generic;

namespace RightFlightWeb.Data
{
    public partial class Flight
    {
        public Flight()
        {
            Booking = new HashSet<Booking>();
        }

        public int FlightId { get; set; }
        public int RouteAircraftId { get; set; }
        public string FlightNumber { get; set; }
        public DateTime ScheduledDeparture { get; set; }

        public virtual RouteAircraft RouteAircraft { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
    }
}
