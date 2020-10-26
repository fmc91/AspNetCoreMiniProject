using Newtonsoft.Json;
using RightFlightWeb.EntityModel;
using RightFlightWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RightFlightWeb.Services
{
    public class FlightInformationService
    {
        public static FlightInformation GetFlightInformation(Flight flight, int adults, int children, int infants)
        {
            string originTimeZoneKey = flight.RouteAircraft.Route.Origin.City.TimeZone;
            string destinationTimeZoneKey = flight.RouteAircraft.Route.Destination.City.TimeZone;

            List<ClassPricingScheme> pricingSchemes =
                JsonConvert.DeserializeObject<List<ClassPricingScheme>>(flight.RouteAircraft.Route.PricingScheme);

            return new FlightInformation
            {
                FlightId = flight.FlightId,
                Origin = Mapper.AirportToDto(flight.RouteAircraft.Route.Origin),
                Destination = Mapper.AirportToDto(flight.RouteAircraft.Route.Destination),
                Airline = flight.RouteAircraft.Route.Airline.Name,
                AirlineLogoFilename = flight.RouteAircraft.Route.Airline.LogoFilename,
                Date = flight.ScheduledDeparture.Date,
                DepartureTime = flight.ScheduledDeparture.TimeOfDay,
                ArrivalTime = ArrivalTimeService.Calculate(flight.ScheduledDeparture, flight.RouteAircraft.FlightDuration, originTimeZoneKey, destinationTimeZoneKey).TimeOfDay,
                FlightDuration = TimeSpan.FromMinutes(flight.RouteAircraft.FlightDuration),
                FlightNumber = flight.FlightNumber,
                AircraftType = flight.RouteAircraft.Aircraft.Model,
                TicketPrices = TicketPriceService.Calculate(pricingSchemes, adults, children, infants),
                TravelClasses = pricingSchemes.Select(ps => new TravelClassDto
                {
                    TravelClassCode = ps.TravelClassCode,
                    TravelClassName = ps.TravelClassName
                }).ToList()
            };
        }
    }
}
