using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RightFlightWeb.EntityModel;
using RightFlightWeb.Models;

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
                SearchResults = new List<FlightSearchResult>()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Search(string originCode, string destinationCode, int adults, int children, int infants, DateTime date)
        {
            IQueryable<FlightSearchResult> searchQuery =
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
                .Select(f => Mapper.FlightToSearchResult(f));

            FlightSearchViewModel viewModel = new FlightSearchViewModel
            {
                Adults = adults,
                Children = children,
                Infants = infants,
                Date = date,
                SearchResults = await searchQuery.ToListAsync()
            };

            return View(viewModel);
        }
    }
}
