// Decompiled with JetBrains decompiler
// Type: Seemplexity.Avalon.BusinesLogic.Services.TouristsService
// Assembly: Seemplexity.Avalon.BusinesLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 30BCE204-0124-42D8-B3AE-A0F92B55EC4D
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Avalon.BusinesLogic.dll

using System;
using System.Data.Entity.Core.Objects;

namespace Seemplexity.Avalon.BusinesLogic.Services
{
  public class TouristsService
  {
    public string GetDogovorCode()
    {
      using (Seemplexity.Avalon.BusinesLogic.Avalon avalon = new Seemplexity.Avalon.BusinesLogic.Avalon())
      {
        ObjectParameter name = new ObjectParameter("name", typeof (string));
        avalon.MakePutName(new DateTime?(DateTime.Now.Date), new int?(4), new int?(4), new int?(17787), new int?(), "PCYMMDD999", name);
      }
      return (string) null;
    }
  }
}
