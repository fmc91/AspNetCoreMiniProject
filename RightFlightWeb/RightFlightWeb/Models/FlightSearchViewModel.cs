using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RightFlightWeb.Models
{
    public class FlightSearchViewModel
    {
        [Required]
        [RegularExpression(@"^[A-Z]{3}$")]
        public string OriginCode { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z]{3}$")]
        public string DestinationCode { get; set; }

        [Range(0, 9)]
        public int Adults { get; set; }

        [Range(0, 9)]
        public int Children { get; set; }

        [Range(0, 9)]
        public int Infants { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public List<FlightSearchResult> SearchResults { get; set; }
    }
}
