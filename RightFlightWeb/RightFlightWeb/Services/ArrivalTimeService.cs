using System;
using System.Collections.Generic;
using System.Text;

namespace RightFlightWeb.Services
{
    public static class ArrivalTimeService
    {
        public static DateTime Calculate(DateTime departureTime, int flightDuration, string originTimeZoneKey, string destinationTimeZoneKey)
        {
            DateTime arrivalTimeInOriginTimeZone = departureTime + TimeSpan.FromMinutes(flightDuration);

            DateTime arrivalTimeInDestinationTimeZone =
                TimeZoneInfo.ConvertTimeBySystemTimeZoneId(arrivalTimeInOriginTimeZone, originTimeZoneKey, destinationTimeZoneKey);

            return arrivalTimeInDestinationTimeZone;
        }
    }
}
