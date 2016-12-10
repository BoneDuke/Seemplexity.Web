using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seemplexity.BusinesLogic.Model
{
    public class BusSchemeFloorDescription
    {
        public int Id { get; set; }
        public int FloorNumber { get; set; }
        public int RowsCount { get; set; }
        public int ColumnsCount { get; set; }
        public string Description { get; set; }

        public BusDescription BusDescription { get; set; }

        public virtual ICollection<BusSchemeItem> Items { get; set; }
    }
}
