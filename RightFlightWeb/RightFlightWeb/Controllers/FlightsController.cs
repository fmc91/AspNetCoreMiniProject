using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RightFlightWeb.Models;
using RightFlightWeb.Services;

namespace RightFlightWeb.Controllers
{
    public class FlightsController : Controller
    {
        private readonly FlightInformationService _flightInfoService;

        private readonly PassengerInformationService _passengerInfoService;

        public FlightsController(FlightInformationService flightInfoService, PassengerInformationService passengerInfoService)
        {
            _flightInfoService = flightInfoService;
            _passengerInfoService = passengerInfoService;
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

            List<FlightInformation> searchResults = await _flightInfoService.FlightSearchAsync(originCode, destinationCode, adults, children, infants, date);

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
            if (!await _flightInfoService.FlightExistsAsync(flightId))
                return NotFound();

            FlightInformation flightInformation = await _flightInfoService.GetFlightByIdAsync(flightId, adults, children, infants);

            TravelClassDto travelClass = await _flightInfoService.GetTravelClassByCodeAsync(travelClassCode);

            List<NationalityDto> nationalities = await _passengerInfoService.GetNationalitiesAsync();

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
