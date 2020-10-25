using System;
using System.Collections.Generic;

namespace RightFlightWeb.EntityModel
{
    public partial class Airport
    {
        public Airport()
        {
            DestinationRoute = new HashSet<Route>();
            OriginRoute = new HashSet<Route>();
        }

        public string IataAirportCode { get; set; }
        public string Name { get; set; }
        public string CityCode { get; set; }

        public virtual City City { get; set; }
        public virtual ICollection<Route> DestinationRoute { get; set; }
        public virtual ICollection<Route> OriginRoute { get; set; }
    }
}
