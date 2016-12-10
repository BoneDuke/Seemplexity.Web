using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Seemplexity.Web.Models.Admin
{
    public class BusTourViewModel
    {
        public BusTourViewModel()
        {
            TourTypes = new List<KeyValueViewModel>();
            Tours = new List<KeyValueViewModel>();
        }

        public List<KeyValueViewModel> TourTypes { get; set; }
        public List<KeyValueViewModel> Tours { get; set; }
    }
}