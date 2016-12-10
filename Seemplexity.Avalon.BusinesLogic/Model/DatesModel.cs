using System;
using System.Collections.Generic;

namespace Seemplexity.Avalon.BusinesLogic.Model
{
    public sealed class DatesModel
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public List<DateTime> DisabledDates { get; set; }
        public DateTime? SelectedDate { get; set; }

        public DatesModel()
        {
            SelectedDate = null;
            DisabledDates = new List<DateTime>();
            MinDate = null;
            MaxDate = null;
        }
    }
}
