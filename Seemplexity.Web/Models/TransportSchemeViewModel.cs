using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Seemplexity.Avalon.BusinesLogic.Model;

namespace Seemplexity.Web.Models
{
    public class TransportSchemeViewModel
    {
        public int CityTo { get; set; }
        public int CityFrom { get; set; }
        public string CityToName { get; set; }
        public string CityFromName { get; set; }
        public int ServiceListKey { get; set; }
        public int PartnerKey { get; set; }
        public DateTime? Date { get; set; }
        public TransportScheme Scheme { get; set; }

        public string SelectedPlaces { get; set; }
        public string ServiceListName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Resources))]
        public string PhoneNumber { get; set; }

        public List<TuristViewModel> Turists { get; set; }
    }
}