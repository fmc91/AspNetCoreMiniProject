using System;
using System.Collections.Generic;
using System.Text;

namespace RightFlightWeb
{
    public static class TimeService
    {
        public static DateTime CalculateArrivalTime(DateTime departureTime, int flightDuration, string originTimeZoneKey, string destinationTimeZoneKey)
        {
            DateTime arrivalTimeInOriginTimeZone = departureTime + TimeSpan.FromMinutes(flightDuration);

            DateTime arrivalTimeInDestinationTimeZone =
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(arrivalTimeInOriginTimeZone, originTimeZoneKey, destinationTimeZoneKey);

            return arrivalTimeInDestinationTimeZone;
        }
    }
}
