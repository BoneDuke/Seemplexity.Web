// Decompiled with JetBrains decompiler
// Type: Seemplexity.Common.Helpers.SubstringExtensions
// Assembly: Seemplexity.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 890F1AE1-8FE0-4739-8116-070B8BB405DC
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Common.dll

namespace Seemplexity.Common.Helpers
{
  public static class SubstringExtensions
  {
    public static string Between(this string value, string a, string b)
    {
      int num1 = value.IndexOf(a);
      int num2 = value.LastIndexOf(b);
      if (num1 == -1 || num2 == -1)
        return "";
      int startIndex = num1 + a.Length;
      if (startIndex >= num2)
        return "";
      return value.Substring(startIndex, num2 - startIndex);
    }

    public static string Before(this string value, string a)
    {
      int length = value.IndexOf(a);
      if (length == -1)
        return "";
      return value.Substring(0, length);
    }

    public static string After(this string value, string a)
    {
      int num = value.LastIndexOf(a);
      if (num == -1)
        return "";
      int startIndex = num + a.Length;
      if (startIndex >= value.Length)
        return "";
      return value.Substring(startIndex);
    }
  }
}
