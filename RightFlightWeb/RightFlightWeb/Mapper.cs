using RightFlightWeb.EntityModel;
using RightFlightWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RightFlightWeb
{
    public static class Mapper
    {
        public static AirportDto AirportToDto(Airport airport)
        {
            return new AirportDto
            {
                IataCode = airport.IataAirportCode,
                Name = airport.Name,
                City = airport.City.Name,
                Country = airport.City.Country.Name
            };
        }

        public static FlightSearchResult FlightToSearchResult(Flight flight)
        {
            string originTimeZoneKey = flight.RouteAircraft.Route.Origin.City.TimeZone;
            string destinationTimeZoneKey = flight.RouteAircraft.Route.Destination.City.TimeZone;

            return new FlightSearchResult
            {
                Origin = AirportToDto(flight.RouteAircraft.Route.Origin),
                Destination = AirportToDto(flight.RouteAircraft.Route.Destination),
                Airline = flight.RouteAircraft.Route.Airline.Name,
                AirlineLogoFilename = flight.RouteAircraft.Route.Airline.LogoFilename,
                Date = flight.ScheduledDeparture.Date,
                DepartureTime = flight.ScheduledDeparture.TimeOfDay,
                ArrivalTime = TimeService.CalculateArrivalTime(flight.ScheduledDeparture, flight.RouteAircraft.FlightDuration, originTimeZoneKey, destinationTimeZoneKey).TimeOfDay,
                FlightDuration = TimeSpan.FromMinutes(flight.RouteAircraft.FlightDuration),
                FlightNumber = flight.FlightNumber,
                AircraftType = flight.RouteAircraft.Aircraft.Model
            };
        }
    }
}
