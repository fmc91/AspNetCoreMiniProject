using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RightFlightWeb.Models
{
    public class FlightSearchResult
    {
        public int FlightId { get; set; }

        public AirportDto Origin { get; set; }

        public AirportDto Destination { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dddd d MMMM yyyy}")]
        public DateTime Date { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan DepartureTime { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}")]
        public TimeSpan ArrivalTime { get; set; }

        public TimeSpan FlightDuration { get; set; }

        public string Airline { get; set; }

        public string AirlineLogoFilename { get; set; }

        public string FlightNumber { get; set; }

        public string AircraftType { get; set; }

        public List<TicketPrice> TicketPrices { get; set; }

        public List<TravelClassDto> TravelClasses { get; set; }
    }
}
