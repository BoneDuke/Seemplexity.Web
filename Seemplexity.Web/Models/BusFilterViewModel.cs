using System;

namespace Seemplexity.Web.Models
{
    public class BusFilterViewModel
    {
        public int? CountryTo { get; set; }
        public int? CityFrom { get; set; }
        public int? CityTo { get; set; }
        public DateTime? Date { get; set; }
    }
}