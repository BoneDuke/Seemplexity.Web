using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Converters;

namespace Seemplexity.Web.Utils
{
    public class RuFormatDateTimeConverter : IsoDateTimeConverter
    {
        public RuFormatDateTimeConverter()
        {
            base.DateTimeFormat = "dd.MM.yyyy";
        }
    }
}