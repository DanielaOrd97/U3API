using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace U3API.Helpers
{
    public static class CentralStandarTime
    {
        public static DateTime ToMexicoTime(this DateTime dateTime)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, "Central America Standard Time");
        }
    }
}
