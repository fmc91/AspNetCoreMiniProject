﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RightFlightWeb.Models
{
    public class AirportDto
    {
        public string IataCode { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
    }
}
