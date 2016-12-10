using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seemplexity.Avalon.BusinesLogic.Model;

namespace Seemplexity.Avalon.BusinesLogic.Services
{
    public class VehicleService
    {
        public TransportScheme GetTransportScheme(int transportKey)
        {
            
            using (var context = new Avalon())
            {
                var vehicle = context.Vehicles.Single(v => v.VH_HOSTKEY == transportKey);
                var plan = context.VehiclePlans.Where(vp => vp.VP_VHKEY == vehicle.VH_KEY).OrderBy(vp => vp.VP_VHROW).ToList();
                var result = new TransportScheme();
                if (vehicle.VH_NUMOFAREA != null)
                    result.NumArea = vehicle.VH_NUMOFAREA.Value;
                if (vehicle.VH_NUMOFCOLUMN != null)
                    result.NumColumns = vehicle.VH_NUMOFCOLUMN.Value;
                if (vehicle.VH_NUMOFROW != null)
                    result.NumRows = vehicle.VH_NUMOFROW.Value;

                result.Places = new string[result.NumRows][];
                var currRow = 0;
                foreach (var item in plan)
                {
                    var row = new string[result.NumColumns];
                    if (result.NumColumns > 0)
                        row[0] = item.VP_SEATNUMBER1;
                    if (result.NumColumns > 1)
                        row[1] = item.VP_SEATNUMBER2;
                    if (result.NumColumns > 2)
                        row[2] = item.VP_SEATNUMBER3;
                    if (result.NumColumns > 3)
                        row[3] = item.VP_SEATNUMBER4;
                    if (result.NumColumns > 4)
                        row[4] = item.VP_SEATNUMBER5;
                    if (result.NumColumns > 5)
                        row[5] = item.VP_SEATNUMBER6;

                    result.Places[currRow] = row;

                    currRow++;
                }
                return result;

            }
        }
    }
}
