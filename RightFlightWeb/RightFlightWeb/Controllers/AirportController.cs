using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RightFlightWeb.Data;
using RightFlightWeb.Models;

namespace RightFlightWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly FlightReservationContext _db;

        public AirportController(FlightReservationContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<AirportDto>>> Search(string query)
        {
            if (String.IsNullOrEmpty(query) || query.Length < 3)
                return new List<AirportDto>();

            IQueryable<AirportDto> airportQuery =
                _db.Airport
                .Include(a => a.City)
                .ThenInclude(c => c.Country)
                .Where(a => a.Name.Contains(query) || a.City.Name.Contains(query) || a.IataAirportCode == query)
                .Select(a => Mapper.AirportToDto(a));

            return await airportQuery.ToListAsync();
        }
    }
}
