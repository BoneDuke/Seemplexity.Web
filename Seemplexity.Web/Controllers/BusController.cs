using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Seemplexity.Avalon.BusinesLogic.Services;
using Seemplexity.BusinesLogic.Services;
using Seemplexity.Web.Controllers.Mailers;
using Seemplexity.Web.Filters;
using Seemplexity.Web.ModelBinders;
using Seemplexity.Web.Models;
using Seemplexity.Web.Utils;

namespace Seemplexity.Web.Controllers
{
    [Localized]
    public class BusController : Controller
    {
        private readonly BusDirectionsService _busDirectionsService;
        private readonly TransportService _transportService;
        private readonly VehicleService _vehicleService;

        public BusController()
        {
            var busServiceKey = int.Parse(SettingsManager.ApplicationSettings[SettingsManager.BusServiceKey]);
            var busTourTypeKey = SettingsManager.GetSettingListValue(SettingsManager.BusTourType);
            _busDirectionsService = new BusDirectionsService(busServiceKey, busTourTypeKey);
            _transportService = new TransportService();
            _vehicleService = new VehicleService();
        }

        public ActionResult Index()
        {
            return RedirectToAction("Search");
        }

        // GET: Bus
        public ActionResult Search([ModelBinder(typeof(BusFilterViewModelBinder))] BusFilterViewModel filter)
        {
            if (filter.CountryTo == null && filter.CityTo == null && filter.CityFrom == null && filter.Date == null)
                return View(new BusViewModel());

            var countriesTo = _busDirectionsService.GetCountriesTo()
                .Select(c => new SelectListItem()
                {
                    Selected = c.Key == filter.CountryTo,
                    Text =  c.Value.Actual,
                    Value = c.Key.ToString()
                }).ToList();
            var selectedCountryTo = countriesTo.SingleOrDefault(c => c.Selected);
            var selectedCountryToKey =
                int.Parse(selectedCountryTo != null ? selectedCountryTo.Value : countriesTo[0].Value);

            var citiesFrom = _busDirectionsService.GetCitiesFrom(selectedCountryToKey)
                .Select(c => new SelectListItem()
                {
                    Selected = c.Key == filter.CityFrom,
                    Text = c.Value.Actual,
                    Value = c.Key.ToString()
                }).ToList();
            var selectedCityFrom = citiesFrom.SingleOrDefault(c => c.Selected);
            var selectedCityFromKey = int.Parse(selectedCityFrom != null ? selectedCityFrom.Value : citiesFrom[0].Value);

            var citiesTo = _busDirectionsService.GetCitiesTo(selectedCountryToKey, selectedCityFromKey)
                .Select(c => new SelectListItem()
                {
                    Selected = c.Key == filter.CityTo,
                    Text = c.Value.Actual,
                    Value = c.Key.ToString()
                }).ToList();
            var selectedCityTo = citiesTo.SingleOrDefault(c => c.Selected);
            var selectedCityToKey = int.Parse(selectedCityTo != null ? selectedCityTo.Value : citiesTo[0].Value);

            var datesModel = _busDirectionsService.GetDates(selectedCountryToKey, selectedCityFromKey, selectedCityToKey, filter.Date);
            if (datesModel.SelectedDate != null)
            {
                var model = new BusViewModel()
                {
                    CountriesTo = countriesTo,
                    CitiesFrom = citiesFrom,
                    CitiesTo = citiesTo,
                    DatesModel = datesModel,
                    SearchResult = _busDirectionsService.SearchResult(selectedCountryToKey, selectedCityFromKey, selectedCityToKey, datesModel.SelectedDate.Value, Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName)
                };

                foreach (var res in model.SearchResult)
                {
                    res.HasDescription = _transportService.HasBusDescription(res.Date, res.PartnerKey,
                        res.ServiceListKey);
                }

                return View(model);
            }

            return View(new BusViewModel());
        }

        public ActionResult BusDescriptions()
        {
            var model = _transportService.GetBusDescriptions(true);
            return View(model);
        }

        public ActionResult BusDescription([ModelBinder(typeof(DateModelBinder))] DateTime date, int partnerKey, int serviceListKey)
        {
            var model = _transportService.GetBusDescription(date, partnerKey, serviceListKey, true);
            return PartialView(model);
        }

        public ActionResult TransportScheme([ModelBinder(typeof(TransportSchemeViewModelBinder))] TransportSchemeViewModel model)
        {
            var transportKey = _transportService.GetTransportKey(model.ServiceListKey, model.PartnerKey, model.Date ?? DateTime.MinValue);
            if (transportKey != null)
            {
                model.Scheme = _vehicleService.GetTransportScheme(transportKey.Value);
                return PartialView(model);
            }
            else
            {
                return PartialView(null);
            }
        }

        public ActionResult Book([ModelBinder(typeof(TransportSchemeViewModelBinder))] TransportSchemeViewModel model)
        {
            if (model.Turists == null || model.Turists.Count == 0)
            {
                ModelState.Clear();
                var selectedPlacesList = model.SelectedPlaces.Split(',');
                model.Turists = new List<TuristViewModel>(selectedPlacesList.Length);
                foreach (var place in selectedPlacesList)
                {
                    model.Turists.Add(new TuristViewModel()
                    {
                        PlaceNumber = place
                    });
                }

                var cityTo = _busDirectionsService.GetCityTo(model.CityTo);
                var cityFrom = _busDirectionsService.GetCityFrom(model.CityFrom);
                model.CityFromName = cityFrom.Value;
                model.CityToName = cityTo.Value;
            }
            else if (ModelState.IsValid)
            {
                var success = SendBooking(model);
                if (success)
                    return View("Success");
            }
            
            return View(model);
        }



        [System.Web.Mvc.HttpPost]
        public bool SendBooking([ModelBinder(typeof(TransportSchemeViewModelBinder))] TransportSchemeViewModel model)
        {
            var selectedPlacesList = model.SelectedPlaces.Split(',');

            var mailer = new AccountMailer();
            var emailModel = new EmailModel()
            {
                To = new List<string>()
                {
                    ConfigurationManager.AppSettings["BookingEmailTo"]
                },
                Data = new Dictionary<string, string>()
                {
                    {"SelectedPlaces", model.SelectedPlaces},
                    {"SelectedPlacesCount", selectedPlacesList.Length.ToString()},
                    {"Date", model.Date?.ToString("dd.MM.yyyy") ?? string.Empty},
                    {"ServiceListName", model.ServiceListName},
                    {"CityFromName", model.CityFromName},
                    {"CityToName", model.CityToName},
                    {"UserMail", model.Email},
                    {"PhoneNumber", model.PhoneNumber}
                },
                Subject = "Было произведено бронирование",
                Turists = model.Turists
            };

            mailer.BookingEmail(emailModel).Send();

            return true;
        }
    }
}