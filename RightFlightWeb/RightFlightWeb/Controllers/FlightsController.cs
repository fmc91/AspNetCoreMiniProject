using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RightFlightWeb.EntityModel;
using RightFlightWeb.Models;
using RightFlightWeb.Services;

namespace RightFlightWeb.Controllers
{
    public class FlightsController : Controller
    {
        private readonly FlightReservationContext _db;

        public FlightsController(FlightReservationContext context)
        {
            _db = context;
        }

        public IActionResult Search()
        {
            FlightSearchViewModel viewModel = new FlightSearchViewModel
            {
                Adults = 1,
                Children = 0,
                Infants = 0,
                Date = DateTime.Today,
                SearchResults = new List<FlightInformation>()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string originCode, string destinationCode, int adults, int children, int infants, DateTime date)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Search");
            }

            List<FlightInformation> searchResults = await
                _db.Flight
                .Where(f =>
                    f.RouteAircraft.Route.OriginAirportCode == originCode &&
                    f.RouteAircraft.Route.DestinationAirportCode == destinationCode &&
                    f.ScheduledDeparture.Date == date)
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
                    .ThenInclude(c => c.Country)
                .Select(f => FlightInformationService.GetFlightInformation(f, adults, children, infants))
                .ToListAsync();

            FlightSearchViewModel viewModel = new FlightSearchViewModel
            {
                Adults = adults,
                Children = children,
                Infants = infants,
                Date = date,
                SearchResults = searchResults
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Book(int flightId, string travelClassCode, int adults, int children, int infants)
        {
            if (!FlightExists(flightId))
                return NotFound();

            FlightInformation flightInformation = await
                _db.Flight
                .Where(f => f.FlightId == flightId)
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
                    .ThenInclude(c => c.Country)
                .Select(f => FlightInformationService.GetFlightInformation(f, adults, children, infants))
                .SingleAsync();

            TravelClassDto travelClass = await
                _db.TravelClass
                .Where(tc => tc.TravelClassCode == travelClassCode)
                .Select(tc => Mapper.TravelClassToDto(tc))
                .SingleAsync();

            List<NationalityDto> nationalities = await
                _db.Nationality
                .Select(n => Mapper.NationalityToDto(n))
                .ToListAsync();

            BookFlightViewModel viewModel = new BookFlightViewModel
            {
                FlightInformation = flightInformation,
                Passengers = new List<PassengerDto>(),
                TravelClass = travelClass,
                Nationalities = nationalities
            };

            for (int i = 0; i < adults; i++)
                viewModel.Passengers.Add(new PassengerDto { AgeBracket = AgeBracket.Adult, DateOfBirth = DateTime.Today });

            for (int i = 0; i < children; i++)
                viewModel.Passengers.Add(new PassengerDto { AgeBracket = AgeBracket.Child, DateOfBirth = DateTime.Today });

            for (int i = 0; i < infants; i++)
                viewModel.Passengers.Add(new PassengerDto { AgeBracket = AgeBracket.Infant, DateOfBirth = DateTime.Today });

            return View(viewModel);
        }

        private bool FlightExists(int flightId)
        {
            return _db.Flight.Find(flightId) != null;
        }
    }
}
