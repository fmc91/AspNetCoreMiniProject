using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RightFlightWeb.Models
{
    public class TicketPrice
    {
        public string TravelClassCode { get; set; }

        public string TravelClass { get; set; }

        public float Amount { get; set; }
    }
}
