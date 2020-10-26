using System;
using System.Collections.Generic;
using System.Linq;
using RightFlightWeb.EntityModel;
using RightFlightWeb.Models;
using RightFlightWeb.Services;
using Newtonsoft.Json;

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

        public static TravelClassDto TravelClassToDto(TravelClass travelClass)
        {
            return new TravelClassDto
            {
                TravelClassCode = travelClass.TravelClassCode,
                TravelClassName = travelClass.Name
            };
        }

        public static NationalityDto NationalityToDto(Nationality nationality)
        {
            return new NationalityDto
            {
                NationalityId = nationality.NationalityId,
                CountryName = nationality.CountryName
            };
        }
    }
}
