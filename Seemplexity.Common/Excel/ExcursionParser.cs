// Decompiled with JetBrains decompiler
// Type: Seemplexity.Common.Excel.ExcursionParser
// Assembly: Seemplexity.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 890F1AE1-8FE0-4739-8116-070B8BB405DC
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Common.dll

using LinqToExcel;
using Seemplexity.Common.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Seemplexity.Common.Excel
{
  public class ExcursionParser : IExcursionParser
  {
    private readonly Func<RowNoHeader, IDictionary<string, int>, TouristExcursionRow> _rowCreator = (Func<RowNoHeader, IDictionary<string, int>, TouristExcursionRow>) ((r, rows) =>
    {
      TouristExcursionRow touristExcursionRow = new TouristExcursionRow()
      {
        Date = DateTime.ParseExact(r[rows["Date"]].ToString(), "dd.MM.yyyy", (IFormatProvider) CultureInfo.CurrentCulture),
        TicketNumber = r[rows["TicketNumber"]].ToString(),
        ExcursionName = r[rows["ExcursionName"]].ToString(),
        HotelName = r[rows["HotelName"]].ToString().ToUpper(),
        Tourists = ExcursionParser.ParseStringIntoTourists(r[rows["Tourists"]].ToString())
      };
      int result;
      if (int.TryParse(r[rows["AdultsCount"]].ToString(), out result))
        touristExcursionRow.AdultsCount = result;
      if (int.TryParse(r[rows["ChildsCount"]].ToString(), out result))
        touristExcursionRow.ChildsCount = result;
      if (int.TryParse(r[rows["Brutto"]].ToString(), out result))
        touristExcursionRow.Brutto = result;
      return touristExcursionRow;
    });

    private Hashtable FillExcelSheets(string fileName)
    {
      Hashtable hashtable = new Hashtable();
      using (ExcelQueryFactory excelQueryFactory = new ExcelQueryFactory(fileName))
      {
        foreach (string worksheetName in excelQueryFactory.GetWorksheetNames())
        {
          RowNoHeader rowNoHeader = excelQueryFactory.WorksheetNoHeader(worksheetName).FirstOrDefault<RowNoHeader>((Expression<Func<RowNoHeader, bool>>) (r => (string) r[0] == "Дата"));
          if (rowNoHeader != null)
          {
            IDictionary<string, int> dictionary = (IDictionary<string, int>) new Dictionary<string, int>();
            for (int index = 0; index < rowNoHeader.Capacity; ++index)
            {
              if (rowNoHeader[index].ToString() == "Дата")
                dictionary.Add("Date", index);
              else if (rowNoHeader[index].ToString().ToUpper().Contains("НОМЕР"))
                dictionary.Add("TicketNumber", index);
              else if (rowNoHeader[index].ToString().ToUpper().Contains("ЕКСКУРСИЯ"))
                dictionary.Add("ExcursionName", index);
              else if (rowNoHeader[index].ToString().ToUpper().Contains("Бр. взр.".ToUpperInvariant()))
                dictionary.Add("AdultsCount", index);
              else if (rowNoHeader[index].ToString().ToUpper().Contains("Бр. Деца".ToUpperInvariant()))
                dictionary.Add("ChildsCount", index);
              else if (rowNoHeader[index].ToString().ToUpper().Contains("Бруто цена".ToUpperInvariant()))
                dictionary.Add("Brutto", index);
              else if (rowNoHeader[index].ToString().ToUpper().Contains("Хотел".ToUpperInvariant()))
                dictionary.Add("HotelName", index);
            }
            dictionary.Add("Tourists", 11);
            hashtable.Add((object) worksheetName, (object) dictionary);
          }
        }
      }
      return hashtable;
    }

    private static List<TouristItem> ParseStringIntoTourists(string str)
    {
      return ((IEnumerable<string>) str.Split(',')).Select<string, TouristItem>((Func<string, TouristItem>) (s => new TouristItem()
      {
        Name = s.Trim().Before(" "),
        Surname = s.Trim().After(" ")
      })).ToList<TouristItem>();
    }

    private List<List<TouristExcursionRow>> GetTourists(string fileName)
    {
      List<List<TouristExcursionRow>> touristExcursionRowListList = new List<List<TouristExcursionRow>>();
      ExcelQueryFactory excelQueryFactory = new ExcelQueryFactory(fileName);
      Hashtable sheets = this.FillExcelSheets(fileName);
      foreach (object key in (IEnumerable) sheets.Keys)
      {
        object sheet = key;
        List<TouristExcursionRow> list = excelQueryFactory.WorksheetNoHeader((string) sheet).Where<RowNoHeader>((Expression<Func<RowNoHeader, bool>>) (r => (string) r[0] != "" && (string) r[0] != "Дата")).Select<RowNoHeader, TouristExcursionRow>((Expression<Func<RowNoHeader, TouristExcursionRow>>) (r => this._rowCreator(r, (IDictionary<string, int>) sheets[sheet]))).ToList<TouristExcursionRow>().Where<TouristExcursionRow>((Func<TouristExcursionRow, bool>) (r => !string.IsNullOrEmpty(r.ExcursionName))).ToList<TouristExcursionRow>();
        touristExcursionRowListList.Add(list);
      }
      return touristExcursionRowListList;
    }

    public ExcelExcursionModel Parse(string fileName)
    {
      return new ExcelExcursionModel()
      {
        Tourists = this.GetTourists(fileName).SelectMany<List<TouristExcursionRow>, TouristExcursionRow>((Func<List<TouristExcursionRow>, IEnumerable<TouristExcursionRow>>) (t => (IEnumerable<TouristExcursionRow>) t)).ToList<TouristExcursionRow>()
      };
    }
  }
}
