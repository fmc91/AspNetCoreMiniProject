using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RightFlightWeb.Data;
using RightFlightWeb.Models;

namespace RightFlightWeb.Services
{
    public class PassengerInformationService
    {
        private readonly FlightReservationContext _db;

        public PassengerInformationService(FlightReservationContext context)
        {
            _db = context;
        }

        public async Task<List<NationalityDto>> GetNationalitiesAsync()
        {
            return await _db.Nationality
                .Select(n => Mapper.NationalityToDto(n))
                .ToListAsync();
        }
    }
}
