﻿// Decompiled with JetBrains decompiler
// Type: Seemplexity.Web.Models.Admin.CompareExcursionsViewModel
// Assembly: Seemplexity.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CB8C5175-05B1-43CD-A55C-EC28DE8A481E
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Web.dll

using Seemplexity.BusinesLogic.Model;
using Seemplexity.Common;
using System.Collections.Generic;

namespace Seemplexity.Web.Models.Admin
{
  public class CompareExcursionsViewModel
  {
    public IList<SmallIdNameModel> Hotels { get; set; }

    public IList<SmallIdNameModel> Excursions { get; set; }

    public List<ExcursionMapping> CompareExcursionsModel { get; set; }

    public List<HotelMapping> CompareHotelsModel { get; set; }

    public string ExcelFilePath { get; set; }
  }
}
