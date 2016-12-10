using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using Seemplexity.Avalon.BusinesLogic.Model;
using Seemplexity.Avalon.BusinesLogic.Services;
using Seemplexity.Avalon.BusinesLogic.Utils;
using Seemplexity.Web.Filters;
using Seemplexity.Web.Utils;

namespace Seemplexity.Web.Controllers
{
    public class BusFilterController : ApiController
    {
        private readonly BusDirectionsService _busDirectionsService;

        public BusFilterController()
        {
            var busServiceKey = int.Parse(SettingsManager.ApplicationSettings[SettingsManager.BusServiceKey]);
            var busTourTypeKey = SettingsManager.GetSettingListValue(SettingsManager.BusTourType);
            _busDirectionsService = new BusDirectionsService(busServiceKey, busTourTypeKey);
        }

        public List<SelectListItem> GetCountriesTo(string lang, int? countryKey)
        {
            return _busDirectionsService.GetCountriesTo()
                .Select(c => new SelectListItem()
            {
                Selected = c.Key == countryKey,
                Text = lang == "ru" ? c.Value.NameRu : c.Value.NameEn,
                Value = c.Key.ToString()
                })
                .ToList();
        }

        public List<SelectListItem> GetCitiesFrom(string lang, int countryKey, int? cityKeyFrom)
        {
            return _busDirectionsService.GetCitiesFrom(countryKey)
                .Select(c => new SelectListItem()
                {
                    Selected = c.Key == cityKeyFrom,
                    Text = lang == "ru" ? c.Value.NameRu : c.Value.NameEn,
                    Value = c.Key.ToString()
                })
                .ToList();
        }

        public List<SelectListItem> GetCitiesTo(string lang, int countryKey, int cityKeyFrom, int? cityKeyTo)
        {
            return _busDirectionsService.GetCitiesTo(countryKey, cityKeyFrom)
                .Select(c => new SelectListItem()
                {
                    Selected = c.Key == cityKeyTo,
                    Text = lang == "ru" ? c.Value.NameRu : c.Value.NameEn,
                    Value = c.Key.ToString()
                })
                .ToList();
        }

        public DatesModel GetDates(int countryKey, int cityKeyFrom, int cityKeyTo, string date)
        {
            var dateTime = Parsers.ParseDateTime(date);
            return _busDirectionsService.GetDates(countryKey, cityKeyFrom, cityKeyTo, dateTime);
        }
    }
}
