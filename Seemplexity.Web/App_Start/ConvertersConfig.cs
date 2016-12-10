using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Seemplexity.Web.Utils;

namespace Seemplexity.Web
{
    public class ConvertersConfig
    {
        public static void RegisterConverters()
        {
            var jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            var jSettings = new Newtonsoft.Json.JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            jSettings.Converters.Add(new RuFormatDateTimeConverter());
            jsonFormatter.SerializerSettings = jSettings;
        }
    }
}