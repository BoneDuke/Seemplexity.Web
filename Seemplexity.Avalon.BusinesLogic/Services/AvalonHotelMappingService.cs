// Decompiled with JetBrains decompiler
// Type: Seemplexity.Avalon.BusinesLogic.Services.AvalonHotelMappingService
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
  public class AvalonHotelMappingService
  {
        public IList<SmallIdNameModel> GetHotelsKeyName(int countryKey)
        {
            using (var context = new Avalon())
            {
                return context.HotelDictionaries
                    .Where(h => h.HD_CNKEY == countryKey)
                    .Select(h => new SmallIdNameModel
                    {
                        Id = h.HD_KEY,
                        Name = h.HD_NAME,
                        NameLat = h.HD_NAMELAT
                    })
                    .ToList();
            }
        }

        public void MapTouristRows(ExcelTransferModel model)
        {
            using (var context = new Avalon())
            {
                foreach (var tourists in model.Tourists)
                {
                    var hotels = tourists.Where(r => r.AvalonHotelKey == null).Select(r => r.HotelName).Distinct().ToList();
                    var avalonHotels = context.HotelDictionaries.Where(h => hotels.Contains(h.HD_NAME.ToUpper()) || hotels.Contains(h.HD_NAMELAT.ToUpper()))
                        .Select(h => new
                        {
                            Id = h.HD_KEY,
                            Name = h.HD_NAME.ToUpper(),
                            NameLat = h.HD_NAMELAT.ToUpper()
                        })
                        .ToList();

                    foreach (var avalonHotel in avalonHotels)
                    {
                        foreach (var tourist in tourists.Where(t => t.AvalonHotelKey == null && (t.HotelName == avalonHotel.Name || t.HotelName == avalonHotel.NameLat)))
                        {
                            tourist.AvalonHotelName = $"({avalonHotel.Name} / {avalonHotel.NameLat}";
                            tourist.AvalonHotelKey = avalonHotel.Id;
                        }
                    }

                    var hotelKeys = tourists.Where(r => r.AvalonHotelKey != null).Select(r => r.AvalonHotelKey).Distinct().ToList();
                    var avalonNames = context.HotelDictionaries.Where(h => hotelKeys.Contains(h.HD_KEY)).ToList();
                    foreach (var tourist in tourists.Where(t => t.AvalonHotelKey != null))
                    {
                        var avalonName = avalonNames.SingleOrDefault(n => tourist.AvalonHotelKey != null && n.HD_KEY == tourist.AvalonHotelKey.Value);
                        if (avalonName != null)
                            tourist.AvalonHotelName = $"{avalonName.HD_NAME} / {avalonName.HD_NAMELAT}";
                    }
                }

            }
        }


        public void MapTouristRows(ExcelExcursionModel model)
    {
      using (Seemplexity.Avalon.BusinesLogic.Avalon avalon = new Seemplexity.Avalon.BusinesLogic.Avalon())
      {
        List<string> hotels = model.Tourists.Where<TouristExcursionRow>((Func<TouristExcursionRow, bool>) (r => !r.AvalonHotelKey.HasValue)).Select<TouristExcursionRow, string>((Func<TouristExcursionRow, string>) (r => r.HotelName)).Distinct<string>().ToList<string>();
        IQueryable<HotelDictionary> source = avalon.HotelDictionaries.Where<HotelDictionary>((Expression<Func<HotelDictionary, bool>>) (h => hotels.Contains(h.HD_NAME.ToUpper()) || hotels.Contains(h.HD_NAMELAT.ToUpper())));
        //Expression<Func<HotelDictionary, \u003C\u003Ef__AnonymousType0<int, string, string>>> selector = h => new
        //{
        //  Id = h.HD_KEY,
        //  Name = h.HD_NAME.ToUpper(),
        //  NameLat = h.HD_NAMELAT.ToUpper()
        //};
        foreach (var data in source.Select(h => new
        {
            Id = h.HD_KEY,
            Name = h.HD_NAME.ToUpper(),
            NameLat = h.HD_NAMELAT.ToUpper()

        }).ToList())
        {
          var avalonHotel = data;
          foreach (TouristExcursionRow touristExcursionRow in model.Tourists.Where<TouristExcursionRow>((Func<TouristExcursionRow, bool>) (t =>
          {
            if (t.AvalonHotelKey.HasValue)
              return false;
            if (!(t.HotelName == avalonHotel.Name))
              return t.HotelName == avalonHotel.NameLat;
            return true;
          })))
          {
            string str = string.Format("({0} / {1}", (object) avalonHotel.Name, (object) avalonHotel.NameLat);
            touristExcursionRow.AvalonHotelName = str;
            int? nullable = new int?(avalonHotel.Id);
            touristExcursionRow.AvalonHotelKey = nullable;
          }
        }
        List<int?> hotelKeys = model.Tourists.Where<TouristExcursionRow>((Func<TouristExcursionRow, bool>) (r => r.AvalonHotelKey.HasValue)).Select<TouristExcursionRow, int?>((Func<TouristExcursionRow, int?>) (r => r.AvalonHotelKey)).Distinct<int?>().ToList<int?>();
        List<HotelDictionary> list = avalon.HotelDictionaries.Where<HotelDictionary>((Expression<Func<HotelDictionary, bool>>) (h => hotelKeys.Contains((int?) h.HD_KEY))).ToList<HotelDictionary>();
        foreach (TouristExcursionRow touristExcursionRow in model.Tourists.Where<TouristExcursionRow>((Func<TouristExcursionRow, bool>) (t => t.AvalonHotelKey.HasValue)))
        {
          TouristExcursionRow tourist = touristExcursionRow;
          HotelDictionary hotelDictionary = list.SingleOrDefault<HotelDictionary>((Func<HotelDictionary, bool>) (n =>
          {
            if (tourist.AvalonHotelKey.HasValue)
              return n.HD_KEY == tourist.AvalonHotelKey.Value;
            return false;
          }));
          if (hotelDictionary != null)
            tourist.AvalonHotelName = string.Format("{0} / {1}", (object) hotelDictionary.HD_NAME, (object) hotelDictionary.HD_NAMELAT);
        }
      }
    }
  }
}
