// Decompiled with JetBrains decompiler
// Type: Seemplexity.BusinesLogic.Model.HotelMapping
// Assembly: Seemplexity.BusinesLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B531B5A1-5C49-4853-9EEE-68D88BB22861
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.BusinesLogic.dll

using Seemplexity.Common.Excel;

namespace Seemplexity.BusinesLogic.Model
{
  public class HotelMapping
  {
    public int Id { get; set; }

    public PartnerType PartnerType { get; set; }

    public string ResortName { get; set; }

    public string HotelName { get; set; }

    public int AvalonHotelKey { get; set; }
  }
}
