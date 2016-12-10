// Decompiled with JetBrains decompiler
// Type: Seemplexity.Avalon.BusinesLogic.Services.AvalonExcursionMappingService
// Assembly: Seemplexity.Avalon.BusinesLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 30BCE204-0124-42D8-B3AE-A0F92B55EC4D
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Avalon.BusinesLogic.dll

using Seemplexity.Common;
using Seemplexity.Common.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Seemplexity.Avalon.BusinesLogic.Services
{
  public class AvalonExcursionMappingService
  {
        public IList<SmallIdNameModel> GetExcursionsKeyName(int countryKey)
        {
            using (var context = new Avalon())
            {
                return context.ExcurDictionaries
                    .Where(h => h.ED_CNKEY == countryKey)
                    .Select(h => new SmallIdNameModel
                    {
                        Id = h.ED_KEY,
                        Name = h.ED_NAME,
                        NameLat = h.ED_NAMELAT
                    })
                    .ToList();
            }
        }


        public void MapTouristRows(ExcelExcursionModel model)
        {
            using (var context = new Avalon())
            {
                var excursions = model.Tourists.Where(r => r.AvalonExcursionKey == null).Select(r => r.ExcursionName).Distinct().ToList();
                var avalonExcursions = context.ExcurDictionaries.Where(h => excursions.Contains(h.ED_NAME.ToUpper()) || excursions.Contains(h.ED_NAMELAT.ToUpper()))
                        .Select(h => new
                        {
                            Id = h.ED_KEY,
                            Name = h.ED_NAME.ToUpper(),
                            NameLat = h.ED_NAMELAT.ToUpper()
                        })
                        .ToList();

                foreach (var avalonExcursion in avalonExcursions)
                {
                    foreach (var tourist in model.Tourists.Where(t => t.AvalonExcursionKey == null && (t.ExcursionName == avalonExcursion.Name || t.ExcursionName == avalonExcursion.NameLat)))
                    {
                        tourist.AvalonExcursionName = $"({avalonExcursion.Name} / {avalonExcursion.NameLat}";
                        tourist.AvalonExcursionKey = avalonExcursion.Id;
                    }
                }


                var hotels = model.Tourists.Where(r => r.AvalonHotelKey == null).Select(r => r.HotelName).Distinct().ToList();
                var avalonHotels = context.HotelDictionaries.Where(h => excursions.Contains(h.HD_NAME.ToUpper()) || excursions.Contains(h.HD_NAMELAT.ToUpper()))
                        .Select(h => new
                        {
                            Id = h.HD_KEY,
                            Name = h.HD_NAME.ToUpper(),
                            NameLat = h.HD_NAMELAT.ToUpper()
                        })
                        .ToList();

                foreach (var avalonHotel in avalonHotels)
                {
                    foreach (var tourist in model.Tourists.Where(t => t.AvalonHotelKey == null && (t.HotelName == avalonHotel.Name || t.HotelName == avalonHotel.NameLat)))
                    {
                        tourist.AvalonHotelName = $"({avalonHotel.Name} / {avalonHotel.NameLat}";
                        tourist.AvalonHotelKey = avalonHotel.Id;
                    }
                }

                

            }
        }

    }
}
