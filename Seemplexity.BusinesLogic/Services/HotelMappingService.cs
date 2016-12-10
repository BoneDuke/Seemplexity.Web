// Decompiled with JetBrains decompiler
// Type: Seemplexity.BusinesLogic.Services.HotelMappingService
// Assembly: Seemplexity.BusinesLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B531B5A1-5C49-4853-9EEE-68D88BB22861
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.BusinesLogic.dll

using Seemplexity.BusinesLogic.Model;
using Seemplexity.Common.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Seemplexity.BusinesLogic.Services
{
  public class HotelMappingService
  {
    public void MapTouristRows(ExcelTransferModel model)
    {
      using (SeemplexityModel seemplexityModel = new SeemplexityModel())
      {
        foreach (List<TouristTransferRow> tourist in model.Tourists)
        {
          List<string> hotels = tourist.Select<TouristTransferRow, string>((Func<TouristTransferRow, string>) (r => r.HotelName)).Distinct<string>().ToList<string>();
          List<HotelMapping> list = seemplexityModel.HotelMappings.Where<HotelMapping>((Expression<Func<HotelMapping, bool>>) (m => hotels.Contains(m.HotelName))).ToList<HotelMapping>();
          if (list.Count == 0)
            break;
          foreach (TouristTransferRow touristTransferRow in tourist)
          {
            TouristTransferRow row = touristTransferRow;
            HotelMapping hotelMapping = list.SingleOrDefault<HotelMapping>((Func<HotelMapping, bool>) (m =>
            {
              if (m.HotelName == row.HotelName && m.ResortName == row.Resort)
                return m.PartnerType == model.Type;
              return false;
            }));
            if (hotelMapping != null)
              row.AvalonHotelKey = new int?(hotelMapping.AvalonHotelKey);
          }
        }
      }
    }

    public void MapTouristRows(ExcelExcursionModel model)
    {
      using (SeemplexityModel seemplexityModel = new SeemplexityModel())
      {
        List<string> hotels = model.Tourists.Select<TouristExcursionRow, string>((Func<TouristExcursionRow, string>) (r => r.HotelName)).Distinct<string>().ToList<string>();
        List<HotelMapping> list = seemplexityModel.HotelMappings.Where<HotelMapping>((Expression<Func<HotelMapping, bool>>) (m => hotels.Contains(m.HotelName))).ToList<HotelMapping>();
        if (list.Count == 0)
          return;
        foreach (TouristExcursionRow tourist in model.Tourists)
        {
          TouristExcursionRow row = tourist;
          HotelMapping hotelMapping = list.SingleOrDefault<HotelMapping>((Func<HotelMapping, bool>) (m =>
          {
            if (m.HotelName == row.HotelName && m.ResortName == string.Empty)
              return m.PartnerType == PartnerType.Excursion;
            return false;
          }));
          if (hotelMapping != null)
            row.AvalonHotelKey = new int?(hotelMapping.AvalonHotelKey);
        }
      }
    }

    public void UpdateHotelMappings(IList<HotelMapping> compareHotels)
    {
      using (SeemplexityModel seemplexityModel = new SeemplexityModel())
      {
        List<string> hotelNames = compareHotels.Select<HotelMapping, string>((Func<HotelMapping, string>) (ch => ch.HotelName)).Distinct<string>().ToList<string>();
        List<HotelMapping> list = seemplexityModel.HotelMappings.Where<HotelMapping>((Expression<Func<HotelMapping, bool>>) (m => hotelNames.Contains(m.HotelName))).ToList<HotelMapping>();
        foreach (HotelMapping compareHotel1 in (IEnumerable<HotelMapping>) compareHotels)
        {
          HotelMapping compareHotel = compareHotel1;
          HotelMapping hotelMapping = list.SingleOrDefault<HotelMapping>((Func<HotelMapping, bool>) (m =>
          {
            if (m.HotelName == compareHotel.HotelName && m.PartnerType == compareHotel.PartnerType)
              return m.ResortName == compareHotel.ResortName;
            return false;
          }));
          if (hotelMapping == null)
            seemplexityModel.HotelMappings.Add(new HotelMapping()
            {
              AvalonHotelKey = compareHotel.AvalonHotelKey,
              HotelName = compareHotel.HotelName,
              ResortName = compareHotel.ResortName,
              PartnerType = compareHotel.PartnerType
            });
          else
            hotelMapping.AvalonHotelKey = compareHotel.AvalonHotelKey;
        }
        seemplexityModel.SaveChanges();
      }
    }
  }
}
