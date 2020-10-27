using System;
using System.Collections.Generic;

namespace RightFlightWeb.Data
{
    public partial class Booking
    {
        public Booking()
        {
            Passenger = new HashSet<Passenger>();
        }

        public int BookingId { get; set; }
        public int FlightId { get; set; }
        public string TravelClassCode { get; set; }
        public string BookingReference { get; set; }
        public decimal Cost { get; set; }
        public DateTime TimeOfBooking { get; set; }

        public virtual Flight Flight { get; set; }
        public virtual TravelClass TravelClassCodeNavigation { get; set; }
        public virtual ICollection<Passenger> Passenger { get; set; }
    }
}
