using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RightFlightWeb.Models
{
    public class TicketPrice
    {
        public string TravelClassCode { get; set; }

        public string TravelClass { get; set; }

        [DisplayFormat(DataFormatString = "£{0:N0}")]
        public float Amount { get; set; }
    }
}
