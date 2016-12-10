using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seemplexity.Avalon.BusinesLogic.Model
{
    public class TransportScheme
    {
        public string Title { get; set; }
        public int NumArea { get; set; }
        public int NumRows { get; set; }
        public int NumColumns { get; set; }

        public string[][] Places { get; set; }
    }
}
