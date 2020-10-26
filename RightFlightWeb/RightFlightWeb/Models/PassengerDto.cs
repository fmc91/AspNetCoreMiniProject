using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RightFlightWeb.Models
{
    public class PassengerDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public AgeBracket AgeBracket { get; set; }

        public int NationalityId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public string AddressLineOne { get; set; }

        public string AddressLineTwo { get; set; }

        public string City { get; set; }

        public string Country { get; set; }


    }
}
