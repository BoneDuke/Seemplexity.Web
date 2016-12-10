// Decompiled with JetBrains decompiler
// Type: Seemplexity.BusinesLogic.Services.ExcursionMappingService
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
  public class ExcursionMappingService
  {
    public void MapTouristRows(ExcelExcursionModel model)
    {
      using (SeemplexityModel seemplexityModel = new SeemplexityModel())
      {
        List<string> excursions = model.Tourists.Select<TouristExcursionRow, string>((Func<TouristExcursionRow, string>) (r => r.ExcursionName)).Distinct<string>().ToList<string>();
        List<ExcursionMapping> list = seemplexityModel.ExcursionMappings.Where<ExcursionMapping>((Expression<Func<ExcursionMapping, bool>>) (m => excursions.Contains(m.ExcursionName))).ToList<ExcursionMapping>();
        if (list.Count == 0)
          return;
        foreach (TouristExcursionRow tourist in model.Tourists)
        {
          TouristExcursionRow row = tourist;
          ExcursionMapping excursionMapping = list.FirstOrDefault<ExcursionMapping>((Func<ExcursionMapping, bool>) (m => m.ExcursionName == row.ExcursionName));
          if (excursionMapping != null)
            row.AvalonExcursionKey = new int?(excursionMapping.AvalonExcursionKey);
        }
      }
    }

    public void UpdateHotelMappings(IList<ExcursionMapping> compareExcursions)
    {
      using (SeemplexityModel seemplexityModel = new SeemplexityModel())
      {
        List<string> excursionNames = compareExcursions.Select<ExcursionMapping, string>((Func<ExcursionMapping, string>) (ch => ch.ExcursionName)).Distinct<string>().ToList<string>();
        List<ExcursionMapping> list = seemplexityModel.ExcursionMappings.Where<ExcursionMapping>((Expression<Func<ExcursionMapping, bool>>) (m => excursionNames.Contains(m.ExcursionName))).ToList<ExcursionMapping>();
        foreach (ExcursionMapping compareExcursion1 in (IEnumerable<ExcursionMapping>) compareExcursions)
        {
          ExcursionMapping compareExcursion = compareExcursion1;
          ExcursionMapping excursionMapping = list.SingleOrDefault<ExcursionMapping>((Func<ExcursionMapping, bool>) (m => m.ExcursionName == compareExcursion.ExcursionName));
          if (excursionMapping == null)
            seemplexityModel.ExcursionMappings.Add(new ExcursionMapping()
            {
              AvalonExcursionKey = compareExcursion.AvalonExcursionKey,
              ExcursionName = compareExcursion.ExcursionName
            });
          else
            excursionMapping.AvalonExcursionKey = compareExcursion.AvalonExcursionKey;
        }
        seemplexityModel.SaveChanges();
      }
    }
  }
}
