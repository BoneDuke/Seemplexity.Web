using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seemplexity.BusinesLogic.Model;

namespace Seemplexity.BusinesLogic.Services
{
    public class TransportService
    {
        public int? GetTransportKey(int serviceListKey, int partnerKey, DateTime date)
        {
            using (var context = new SeemplexityModel())
            {
                var item = context.BusDescriptions.SingleOrDefault(d => d.ServiceListKey == serviceListKey
                    && d.PartnerKey == partnerKey
                    && d.ScheduleDates.Select(s => s.Date).Contains(date));
                return item?.TransportKey ?? 62;
                //var item = context.ServiceListToTransports.SingleOrDefault(s => s.ServiceListKey == serviceListKey);
                //return item?.TransportKey;
            }
        }

        public BusDescription[] GetBusDescriptions(bool includeImages = false)
        {
            using (var context = new SeemplexityModel())
            {
                var query = context.BusDescriptions.AsQueryable();
                if (includeImages)
                    query = query.Include(d => d.Images);
                return query.ToArray();

            }
        }

        public BusDescription GetBusDescription(DateTime date, int partnerKey, int serviceListKey, bool loadImages)
        {
            using (var context = new SeemplexityModel())
            {
                var query = context.BusDescriptions.AsQueryable();
                if (loadImages)
                    query = query.Include(d => d.Images);
                return query.SingleOrDefault(d => d.PartnerKey == partnerKey && d.ServiceListKey == serviceListKey && d.ScheduleDates.Any(sd => sd.Date == date));
            }
        }

        public bool HasBusDescription(DateTime date, int partnerKey, int serviceListKey)
        {
            using (var context = new SeemplexityModel())
            {
                return context.BusDescriptions.Any(d => d.PartnerKey == partnerKey && d.ServiceListKey == serviceListKey && d.ScheduleDates.Any(sd => sd.Date == date));
            }
        }
    }
}
