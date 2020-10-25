using RightFlightWeb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RightFlightWeb.Services
{
    public static class TicketPriceService
    {
        public static List<TicketPrice> Calculate(List<ClassPricingScheme> classPricingSchemes,
                                                              int adults, int children, int infants)
        {
            List<TicketPrice> ticketPrices = new List<TicketPrice>();

            foreach (ClassPricingScheme pricingScheme in classPricingSchemes)
            {
                float amount = adults * pricingScheme.AdultFare +
                              children * pricingScheme.ChildFare +
                              infants * pricingScheme.InfantFare;

                ticketPrices.Add(new TicketPrice
                {
                    TravelClassCode = pricingScheme.TravelClassCode,
                    TravelClass = pricingScheme.TravelClassName,
                    Amount = amount
                });
            }

            return ticketPrices;
        }
    }
}
