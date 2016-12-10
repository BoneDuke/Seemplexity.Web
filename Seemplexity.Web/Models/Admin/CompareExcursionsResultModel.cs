// Decompiled with JetBrains decompiler
// Type: Seemplexity.Web.Models.Admin.CompareExcursionsResultModel
// Assembly: Seemplexity.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CB8C5175-05B1-43CD-A55C-EC28DE8A481E
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Web.dll

using Seemplexity.BusinesLogic.Model;
using System.Collections.Generic;

namespace Seemplexity.Web.Models.Admin
{
  public class CompareExcursionsResultModel
  {
    public IList<ExcursionMapping> CompareExcursions { get; set; }

    public IList<HotelMapping> CompareHotels { get; set; }

    public string ExcelFilePath { get; set; }
  }
}
