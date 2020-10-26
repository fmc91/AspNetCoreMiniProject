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

        private readonly FlightInformationService _flightInformationService;

        public FlightsController(FlightReservationContext context, FlightInformationService flightInformationService)
        {
            _db = context;
            _flightInformationService = flightInformationService;
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
                return NotFound();
            }

            List<FlightInformation> searchResults = await _flightInformationService.SearchAsync(originCode, destinationCode, adults, children, infants, date);

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
            if (!await _flightInformationService.FlightExistsAsync(flightId))
                return NotFound();

            FlightInformation flightInformation = await _flightInformationService.GetByIdAsync(flightId, adults, children, infants);

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

        
    }
}
