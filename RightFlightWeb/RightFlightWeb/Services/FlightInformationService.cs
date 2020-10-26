using Microsoft.EntityFrameworkCore;
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
        private readonly FlightReservationContext _db;

        public FlightInformationService(FlightReservationContext context)
        {
            _db = context;
        }

        private IQueryable<Flight> FlightQuery =>
            _db.Flight
                .Include(f => f.RouteAircraft)
                    .ThenInclude(ra => ra.Route)
                    .ThenInclude(r => r.Airline)
                .Include(f => f.RouteAircraft)
                    .ThenInclude(ra => ra.Aircraft)
                .Include(f => f.RouteAircraft)
                    .ThenInclude(ra => ra.Route)
                    .ThenInclude(r => r.Origin)
                    .ThenInclude(o => o.City)
                    .ThenInclude(c => c.Country)
                .Include(f => f.RouteAircraft)
                    .ThenInclude(ra => ra.Route)
                    .ThenInclude(r => r.Destination)
                    .ThenInclude(d => d.City)
                    .ThenInclude(c => c.Country);

        public async Task<bool> FlightExistsAsync(int flightId)
        {
            return (await _db.Flight.FindAsync(flightId)) != null;
        }

        public async Task<List<FlightInformation>> SearchAsync(string originCode, string destinationCode, int adults, int children, int infants, DateTime date)
        {
            return await FlightQuery
                .Where(f =>
                    f.RouteAircraft.Route.OriginAirportCode == originCode &&
                    f.RouteAircraft.Route.DestinationAirportCode == destinationCode &&
                    f.ScheduledDeparture.Date == date)
                .Select(f => GenerateFlightInformation(f, adults, children, infants))
                .ToListAsync();
        }

        public async Task<FlightInformation> GetByIdAsync(int flightId, int adults, int children, int infants)
        {
            return await FlightQuery
                .Where(f => f.FlightId == flightId)
                .Select(f => GenerateFlightInformation(f, adults, children, infants))
                .SingleAsync();
        }

        private static FlightInformation GenerateFlightInformation(Flight flight, int adults, int children, int infants)
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
