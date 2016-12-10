// Decompiled with JetBrains decompiler
// Type: Seemplexity.Common.Helpers.Excel.Utils
// Assembly: Seemplexity.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 890F1AE1-8FE0-4739-8116-070B8BB405DC
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Common.dll

using Seemplexity.Common.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seemplexity.Common.Helpers.Excel
{
  public static class Utils
  {
    private static readonly Dictionary<string, PartnerType> PartnerTypes = new Dictionary<string, PartnerType>()
    {
      {
        "Glori",
        PartnerType.Gloria
      },
      {
        "Mari",
        PartnerType.Mari
      },
      {
        "Save",
        PartnerType.Save
      },
      {
        "Vision",
        PartnerType.Vision
      },
      {
        "Word",
        PartnerType.Word
      }
    };

    public static PartnerType GetPartnerTypeByFileName(string fileName)
    {
      PartnerType partnerType = PartnerType.Undefined;
      string index = Utils.PartnerTypes.Keys.SingleOrDefault<string>(new Func<string, bool>(fileName.Contains));
      if (!string.IsNullOrEmpty(index))
        partnerType = Utils.PartnerTypes[index];
      return partnerType;
    }
  }
}
