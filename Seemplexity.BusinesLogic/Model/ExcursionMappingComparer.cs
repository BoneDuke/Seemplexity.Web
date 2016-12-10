// Decompiled with JetBrains decompiler
// Type: Seemplexity.BusinesLogic.Model.ExcursionMappingComparer
// Assembly: Seemplexity.BusinesLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B531B5A1-5C49-4853-9EEE-68D88BB22861
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.BusinesLogic.dll

using System.Collections.Generic;

namespace Seemplexity.BusinesLogic.Model
{
  public class ExcursionMappingComparer : IEqualityComparer<ExcursionMapping>
  {
    public bool Equals(ExcursionMapping x, ExcursionMapping y)
    {
      if (x.ExcursionName == y.ExcursionName)
        return x.AvalonExcursionKey == y.AvalonExcursionKey;
      return false;
    }

    public int GetHashCode(ExcursionMapping obj)
    {
      return (13 * 7 + obj.ExcursionName.GetHashCode()) * 7 + obj.AvalonExcursionKey.GetHashCode();
    }
  }
}
