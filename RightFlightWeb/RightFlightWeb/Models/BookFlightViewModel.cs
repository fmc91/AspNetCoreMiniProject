using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RightFlightWeb.Models
{
    public class BookFlightViewModel
    {
        public TravelClassDto TravelClass { get; set; }

        public List<NationalityDto> Nationalities { get; set; }

        public FlightInformation FlightInformation { get; set; }

        public List<PassengerDto> Passengers { get; set; }
    }
}
