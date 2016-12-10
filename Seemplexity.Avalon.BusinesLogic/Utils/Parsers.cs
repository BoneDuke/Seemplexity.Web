using System;
using System.Globalization;

namespace Seemplexity.Avalon.BusinesLogic.Utils
{
    public static class Parsers
    {
        public static DateTime? ParseDateTime(string value)
        {
            DateTime? result = null;
            DateTime outDate;
            if (DateTime.TryParseExact(value, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out outDate))
                result = outDate;
            return result;
        }
    }
}