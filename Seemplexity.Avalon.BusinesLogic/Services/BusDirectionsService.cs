using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Seemplexity.Avalon.BusinesLogic.Model;
using Seemplexity.Common;
using Seemplexity.Resources;

namespace Seemplexity.Avalon.BusinesLogic.Services
{
    public class BusDirectionsService
    {
        private readonly IEnumerable<tbl_TurList> _tourLists;
        private readonly int _busServiceKey;
        private readonly CheckQuotaService _checkQuotaService;

        public BusDirectionsService(int busServiceKey, IEnumerable<int> tourListTypes)
        {
            _busServiceKey = busServiceKey;
            _tourLists = GetTourListKeys(busServiceKey, tourListTypes);
            _checkQuotaService = new CheckQuotaService();
        }

        public IList<tbl_TurList> GetTourListKeys(int busServiceKey, IEnumerable<int> tourListTypes)
        {
            using (var context = new Avalon())
            {
                var result = context.tbl_TurList
                    .Where(t => tourListTypes.Contains(t.TL_TIP) &&
                            context.TurServices.Any(ts => ts.TS_TRKEY == t.TL_KEY && ts.TS_SVKEY == busServiceKey)
                            && context.TurDates.Any(td => td.TD_TRKEY == t.TL_KEY))
                    .ToList();

                return result;
            }
        } 

        public List<KeyValuePair<int, Name>> GetCountriesTo()
        {
            var countryKeys = _tourLists.Select(t => t.TL_CNKEY).ToList();
            using (var context = new Avalon())
            {
                var result = context.tbl_Country.Where(c => countryKeys.Contains(c.CN_KEY))
                    .Select(c => new { c.CN_KEY, c.CN_NAME, c.CN_NAMELAT })
                    .ToList()
                    .Select(c => new KeyValuePair<int, Name>(c.CN_KEY, new Name(c.CN_NAME, c.CN_NAMELAT)))
                    .OrderBy(c => c.Value)
                    .ToList();

                return result;
            }
        }

        public List<KeyValuePair<int, Name>> GetCitiesFrom(int countryKey)
        {
            using (var context = new Avalon())
            {
                var tourListKeys = _tourLists.Where(t => t.TL_CNKEY == countryKey).Select(t => t.TL_KEY).ToList();

                var result = context.AddDescript1
                    .Where(d => d.A1_SVKEY == _busServiceKey)
                    .Where(d => context.tbl_Costs.Any(c => c.CS_PKKEY.HasValue && tourListKeys.Contains(c.CS_PKKEY.Value) && c.CS_SVKEY == _busServiceKey && c.CS_SUBCODE1 == d.A1_KEY))
                    .OrderBy(d => d.A1_ShowOrder)
                    .Select(d => new
                    {
                        Key = d.A1_KEY,
                        ValueRu = d.A1_NAME,
                        ValueEn = d.A1_NAMELAT
                    })
                    .ToList()
                    .Select(d => new KeyValuePair<int, Name>(d.Key, new Name(d.ValueRu, d.ValueEn)))
                    .ToList();

                return result;
            }
        }

        public List<KeyValuePair<int, Name>> GetCitiesTo(int countryKey, int cityKeyFrom)
        {
            using (var context = new Avalon())
            {
                var tourListKeys = _tourLists.Where(t => t.TL_CNKEY == countryKey).Select(t => t.TL_KEY).ToList();

                var result = context.AddDescript2
                    .Where(d => d.A2_SVKEY == _busServiceKey)
                    .Where(d => context.tbl_Costs.Any(c => c.CS_PKKEY.HasValue && tourListKeys.Contains(c.CS_PKKEY.Value) && c.CS_SVKEY == _busServiceKey && c.CS_SUBCODE1 == cityKeyFrom && c.CS_SUBCODE2 == d.A2_KEY))
                    .OrderByDescending(d => d.A2_ShowOrder)
                    .Select(d => new
                    {
                        Key = d.A2_KEY,
                        ValueRu = d.A2_NAME,
                        ValueEn = d.A2_NAMELAT
                    })
                    .ToList()
                    .Select(d => new KeyValuePair<int, Name>(d.Key, new Name(d.ValueRu, d.ValueEn)))
                    .ToList();

                return result;
            }
        }


        public DatesModel GetDates(int countryKey, int cityKeyFrom, int cityKeyTo, DateTime? selectedDate)
        {
            using (var context = new Avalon())
            {
                var tourListKeys = _tourLists.Where(t => t.TL_CNKEY == countryKey).Select(t => t.TL_KEY).ToList();

                var startDate = DateTime.Today.AddDays(-100);

                var dates = context.TurDates
                    .Where(d => tourListKeys.Contains(d.TD_TRKEY) && d.TD_DATE >= startDate)
                    .Where(d => context.tbl_Costs.Any(c => c.CS_PKKEY.HasValue && tourListKeys.Contains(c.CS_PKKEY.Value) && c.CS_SVKEY == _busServiceKey && c.CS_SUBCODE1 == cityKeyFrom && c.CS_SUBCODE2 == cityKeyTo && c.CS_PKKEY == d.TD_TRKEY))
                    .Select(d => d.TD_DATE)
                    .ToList();

                var minDate = dates.Min();
                var maxDate = dates.Max();
                var disabledDates = new List<DateTime>(maxDate.Subtract(minDate).Days - dates.Count + 1);
                for (var date = minDate; date <= maxDate; date = date.AddDays(1))
                {
                    if (!dates.Contains(date))
                        disabledDates.Add(date);
                }

                var result = new DatesModel()
                {
                    MinDate = dates.Min(),
                    MaxDate = dates.Max(),
                    DisabledDates = disabledDates,
                    SelectedDate = selectedDate != null && dates.Contains(selectedDate.Value) ? selectedDate.Value : dates.Min()
                };

                return result;
            }
        }

        public KeyValue GetCityFrom(int id)
        {
            using (var context = new Avalon())
            {
                var addDescr = context.AddDescript1.SingleOrDefault(a => a.A1_KEY == id);
                return addDescr != null ? new KeyValue() {Key = addDescr.A1_KEY, Value = addDescr.A1_NAME} : null;
            }
        }

        public KeyValue GetCityTo(int id)
        {
            using (var context = new Avalon())
            {
                var addDescr = context.AddDescript2.SingleOrDefault(a => a.A2_KEY == id);
                return addDescr != null ? new KeyValue() { Key = addDescr.A2_KEY, Value = addDescr.A2_NAME } : null;
            }
        }

        public List<BusDirectionsResultModel> SearchResult(int countryKey, int cityKeyFrom, int cityKeyTo, DateTime date, string lang)
        {
            List<BusDirectionsResultModel> result;
            using (var context = new Avalon())
            {
                var tourListKeys = _tourLists.Where(t => t.TL_CNKEY == countryKey).Select(t => t.TL_KEY).ToList();

                var directionsFrom = context.AddDescript1.Where(a => a.A1_SVKEY == _busServiceKey)
                    .Select(a => new AddDescriptionModel
                    {
                        Key = a.A1_KEY,
                        Name = a.A1_NAME,
                        NameLat = a.A1_NAMELAT,
                        Order = a.A1_ShowOrder
                    }).ToList();

                var directionsTo = context.AddDescript2.Where(a => a.A2_SVKEY == _busServiceKey)
                    .Select(a => new AddDescriptionModel
                    {
                        Key = a.A2_KEY,
                        Name = a.A2_NAME,
                        NameLat = a.A2_NAMELAT,
                        Order = a.A2_ShowOrder
                    }).ToList();

                var cityFrom = directionsFrom.Single(d => d.Key == cityKeyFrom);
                var cityTo = directionsTo.Single(d => d.Key == cityKeyTo);

                //var busCityFrom = cityFrom.Order < cityTo.Order ? directionsFrom.Single(d => d.Order == directionsFrom.Min(ds => ds.Order)) : directionsFrom.Single(d => d.Order == directionsFrom.Max(ds => ds.Order));
                //var busCityTo = cityFrom.Order < cityTo.Order ? directionsTo.Single(d => d.Order == directionsFrom.Max(ds => ds.Order)) : directionsTo.Single(d => d.Order == directionsTo.Min(ds => ds.Order));

                result = (from c in context.tbl_Costs
                          join sl in context.ServiceLists on c.CS_CODE equals sl.SL_KEY
                          where c.CS_PKKEY.HasValue
                                && tourListKeys.Contains(c.CS_PKKEY.Value)
                                && (c.CS_DATE == null || c.CS_DATE <= date)
                                && (c.CS_DATEEND == null || c.CS_DATEEND >= date)
                                //&& c.CS_SVKEY == _busServiceKey
                                && c.CS_SUBCODE1 == cityFrom.Key
                                && c.CS_SUBCODE2 == cityTo.Key
                          select new
                          {
                              TimeFrom = sl.SL_NAME,
                              CityFromKey = cityFrom.Key,
                              CityFromKeyRu = cityFrom.Name,
                              CityFromKeyEn = cityFrom.NameLat,
                              CityToKey = cityTo.Key,
                              CityToKeyRu = cityTo.Name,
                              CityToKeyEn = cityTo.NameLat,
                              ServiceKey = sl.SL_SVKEY,
                              ServiceListKey = sl.SL_KEY,
                              PartnerKey = c.CS_PRKEY,
                              PacketKey = c.CS_PKKEY.Value,
                              Date = date
                          })
                          .ToList()
                          .Select(v => new BusDirectionsResultModel
                          {
                              TimeFrom = v.TimeFrom,
                              CityFrom = new Tuple<int, Name>(v.CityFromKey, new Name(v.CityFromKeyRu, v.CityFromKeyEn)),
                              CityTo = new Tuple<int, Name>(v.CityToKey, new Name(v.CityToKeyRu, v.CityToKeyEn)),
                              ServiceKey = v.ServiceKey,
                              ServiceListKey = v.ServiceListKey,
                              PartnerKey = v.PartnerKey,
                              PacketKey = v.PacketKey,
                              Date = v.Date
                          })
                          .ToList()
                    .Distinct(new BusDirectionsResultModelEqualityComparer())
                    .ToList();

                var rateCode = Settings.Languages.Single(l => l.Code == lang).RateCode;
                var rate = Settings.Rates.Single(r => r.Code == rateCode);

                foreach (var item in result)
                {
                    var netto = new ObjectParameter("Netto", typeof(decimal));
                    var brutto = new ObjectParameter("Brutto", typeof(decimal));
                    var discount = new ObjectParameter("Discount", typeof(decimal));
                    var nettoDetail = new ObjectParameter("NettoDetail", typeof(string));
                    var badRate = new ObjectParameter("sBadRate", typeof(string));
                    var badDate = new ObjectParameter("dtBadDate", typeof(DateTime));
                    var detailed = new ObjectParameter("sDetailed", typeof(string));
                    var spid = new ObjectParameter("nSpid", typeof(int));
                    var useDiscountDays = new ObjectParameter("UseDiscountDays", typeof(int));
                    var isComission = new ObjectParameter("isCommission", typeof(bool));

                    context.GetServiceCost(item.ServiceKey, item.ServiceListKey, item.CityFrom.Item1, item.CityTo.Item1,
                        item.PartnerKey,
                        item.PacketKey, date, 1, rateCode, 1, 0, 0, 0, DateTime.Now.Date, netto, brutto, discount, nettoDetail,
                        badRate,
                        badDate, detailed, spid, useDiscountDays, item.PacketKey, date, 1, true, isComission);

                    item.Price = double.Parse(brutto.Value.ToString());
                    item.Rate = new Name(rate.Name, rate.Name);

                    var quotaResult = _checkQuotaService.CheckServiceQuota(item.ServiceKey, item.ServiceListKey,
                        item.CityFrom.Item1, item.CityTo.Item1,
                        item.PartnerKey, null, date, date);

                    item.QuotaState = quotaResult.QuotaState;
                    item.Places = quotaResult.Places;
                }
            }

            return result;
        }
    }
}
