using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seemplexity.BusinesLogic.Model
{
    public class BusSchemeItem
    {
        public int Id { get; set; }
        public BusSchemeFloorDescription FloorDescription { get; set; }
        public int RowNumber { get; set; }
        public int ColNumber { get; set; }
        public string Value { get; set; }
        public bool IsUsable { get; set; }
    }
}
