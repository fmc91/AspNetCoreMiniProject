using System;
using System.Collections.Generic;

namespace RightFlightWeb.EntityModel
{
    public partial class Passenger
    {
        public int PassengerId { get; set; }
        public int BookingId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int NationalityId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Postcode { get; set; }

        public virtual Booking Booking { get; set; }
        public virtual Nationality Nationality { get; set; }
    }
}
