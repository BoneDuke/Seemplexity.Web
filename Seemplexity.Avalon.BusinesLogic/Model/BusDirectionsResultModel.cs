using Seemplexity.Common;
using System;
using System.Collections.Generic;

namespace Seemplexity.Avalon.BusinesLogic.Model
{
    public class BusDirectionsResultModel
    {
        public string TimeFrom { get; set; }

        public Tuple<int, Name> CityFrom { get; set; }
        public Tuple<int, Name> CityTo { get; set; }
        public double Price { get; set; }
        public Name Rate { get; set; }

        public DateTime Date { get; set; }

        /// <summary>
        /// Статус квотирования
        /// </summary>
        public QuotesStates QuotaState { get; set; }
        /// <summary>
        /// Число свободных мест
        /// </summary>
        public uint Places { get; set; }

        public int ServiceListKey { get; set; }
        public int ServiceKey { get; set; }
        public int PartnerKey { get; set; }
        public int PacketKey { get; set; }

        public bool HasDescription { get; set; }
    }

    public class BusDirectionsResultModelEqualityComparer : IEqualityComparer<BusDirectionsResultModel>
    {
        public bool Equals(BusDirectionsResultModel x, BusDirectionsResultModel y)
        {
            return x.CityFrom.Item1 == y.CityFrom.Item1 && x.CityTo.Item1 == y.CityTo.Item1 &&
                   x.ServiceListKey == y.ServiceListKey && x.ServiceKey == y.ServiceKey
                   && x.PartnerKey == y.PartnerKey && x.PacketKey == y.PacketKey;
        }

        public int GetHashCode(BusDirectionsResultModel obj)
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + obj.CityFrom.Item1;
                hash = hash * 23 + obj.CityTo.Item1;
                hash = hash * 23 + obj.ServiceListKey;
                hash = hash * 23 + obj.ServiceKey;
                hash = hash * 23 + obj.PartnerKey;
                hash = hash * 23 + obj.PacketKey;
                return hash;
            }
        }
    }
}
