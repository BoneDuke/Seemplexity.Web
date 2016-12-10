// Decompiled with JetBrains decompiler
// Type: Seemplexity.Common.Excel.TouristExcursionRow
// Assembly: Seemplexity.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 890F1AE1-8FE0-4739-8116-070B8BB405DC
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Common.dll

using System;
using System.Collections.Generic;

namespace Seemplexity.Common.Excel
{
  public class TouristExcursionRow
  {
    public DateTime Date { get; set; }

    public string TicketNumber { get; set; }

    public string ExcursionName { get; set; }

    public int? AvalonExcursionKey { get; set; }

    public string AvalonExcursionName { get; set; }

    public int AdultsCount { get; set; }

    public int ChildsCount { get; set; }

    public int Brutto { get; set; }

    public string HotelName { get; set; }

    public int? AvalonHotelKey { get; set; }

    public string AvalonHotelName { get; set; }

    public List<TouristItem> Tourists { get; set; }
  }
}
