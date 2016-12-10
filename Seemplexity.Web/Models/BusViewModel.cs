using System.Collections.Generic;
using System.Web.Mvc;
using Seemplexity.Avalon.BusinesLogic.Model;

namespace Seemplexity.Web.Models
{
    public class BusViewModel
    {
        public List<SelectListItem> CountriesTo { get; set; }
        public List<SelectListItem> CitiesFrom { get; set; }
        public List<SelectListItem> CitiesTo { get; set; }
        public DatesModel DatesModel { get; set; }
        public List<BusDirectionsResultModel> SearchResult { get; set; }

        public BusViewModel()
        {
            CountriesTo = new List<SelectListItem>();
            CitiesFrom = new List<SelectListItem>();
            CitiesTo = new List<SelectListItem>();
            DatesModel = new DatesModel();
            SearchResult = new List<BusDirectionsResultModel>();
        }
    }
}