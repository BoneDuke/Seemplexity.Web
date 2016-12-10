// Decompiled with JetBrains decompiler
// Type: Seemplexity.BusinesLogic.Model.HotelMappingComparer
// Assembly: Seemplexity.BusinesLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B531B5A1-5C49-4853-9EEE-68D88BB22861
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.BusinesLogic.dll

using System.Collections.Generic;

namespace Seemplexity.BusinesLogic.Model
{
  public class HotelMappingComparer : IEqualityComparer<HotelMapping>
  {
    public bool Equals(HotelMapping x, HotelMapping y)
    {
      if (x.HotelName == y.HotelName && x.ResortName == y.ResortName && x.PartnerType == y.PartnerType)
        return x.AvalonHotelKey == y.AvalonHotelKey;
      return false;
    }

    public int GetHashCode(HotelMapping obj)
    {
      return (((13 * 7 + obj.AvalonHotelKey.GetHashCode()) * 7 + obj.HotelName.GetHashCode()) * 7 + obj.ResortName.GetHashCode()) * 7 + obj.PartnerType.GetHashCode();
    }
  }
}
