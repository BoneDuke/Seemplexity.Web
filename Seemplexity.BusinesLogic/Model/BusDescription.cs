using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Seemplexity.BusinesLogic.Model
{
    public class BusDescription
    {
        public BusDescription()
        {
            ScheduleDates = new List<BusDescriptionScheduleDates>();
        }

        public int Id { get; set; }

        [Display(Name = "Bus", ResourceType = typeof(Resources.Resources))]
        [StringLength(1024)]
        public string Name { get; set; }

        [Display(Name = "Schedule", ResourceType = typeof(Resources.Resources))]
        [StringLength(1024)]
        public string Schedule { get; set; }

        [Display(Name = "DepartureTime", ResourceType = typeof(Resources.Resources))]
        [StringLength(10)]
        public string DepartureTime { get; set; }

        [Display(Name = "TimeToSeat", ResourceType = typeof(Resources.Resources))]
        [StringLength(10)]
        public string TimeToSeat { get; set; }

        [Display(Name = "DeparturePlace", ResourceType = typeof(Resources.Resources))]
        [StringLength(1024)]
        public string DeparturePlace { get; set; }

        [Display(Name = "IncludeInPrice", ResourceType = typeof(Resources.Resources))]
        [StringLength(1024)]
        public string IncludeInPrice { get; set; }

        [Display(Name = "NotCarried", ResourceType = typeof(Resources.Resources))]
        [StringLength(1024)]
        public string NotCarried { get; set; }

        [Display(Name = "InBus", ResourceType = typeof(Resources.Resources))]
        [StringLength(1024)]
        public string InBus { get; set; }

        [Display(Name = "PartnerKey", ResourceType = typeof(Resources.Resources))]
        public int PartnerKey { get; set; }

        [Display(Name = "ScheduleDates", ResourceType = typeof(Resources.Resources))]
        public virtual ICollection<BusDescriptionScheduleDates> ScheduleDates { get; set; }

        [Display(Name = "ServiceListKey", ResourceType = typeof(Resources.Resources))]
        public int ServiceListKey { get; set; }

        public virtual ICollection<BinaryItem> Images { get; set; }

        [Display(Name = "TransportKey", ResourceType = typeof(Resources.Resources))]
        public int TransportKey { get; set; }

        public virtual IList<BusSchemeFloorDescription> SchemeFloorDescriptions { get; set; }
    }
}
