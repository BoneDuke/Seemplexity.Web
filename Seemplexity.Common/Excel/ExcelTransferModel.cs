// Decompiled with JetBrains decompiler
// Type: Seemplexity.Common.Excel.ExcelTransferModel
// Assembly: Seemplexity.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 890F1AE1-8FE0-4739-8116-070B8BB405DC
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Common.dll

using System;
using System.Collections.Generic;

namespace Seemplexity.Common.Excel
{
  public class ExcelTransferModel
  {
    public List<List<TouristTransferRow>> Tourists { get; set; }

    public PartnerType Type { get; set; }

    public int? AvalonPartnerKey { get; set; }

    public string AvalonPartnerName { get; set; }

    public DateTime TourDate { get; set; }
  }
}
