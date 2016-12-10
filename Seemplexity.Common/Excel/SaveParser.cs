// Decompiled with JetBrains decompiler
// Type: Seemplexity.Common.Excel.SaveParser
// Assembly: Seemplexity.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 890F1AE1-8FE0-4739-8116-070B8BB405DC
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Common.dll

using LinqToExcel;
using Remotion.Data.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Seemplexity.Common.Excel
{
  public class SaveParser : ITransferParser
  {
    private readonly Func<RowNoHeader, IDictionary<string, int>, TouristTransferRow> _rowCreator = (Func<RowNoHeader, IDictionary<string, int>, TouristTransferRow>) ((r, rows) =>
    {
      TouristTransferRow touristTransferRow = new TouristTransferRow()
      {
        Name = r[rows["Name"]].ToString().Trim().ToUpper(),
        Surname = r[rows["Surname"]].ToString().Trim().ToUpper(),
        Resort = r[rows["Resort"]].ToString().Trim().ToUpper(),
        HotelName = r[rows["HotelName"]].ToString().Trim().ToUpper()
      };
      int result;
      touristTransferRow.Id = !int.TryParse((string) r[rows["Id"]], out result) ? -1 : result;
      return touristTransferRow;
    });

    private Hashtable FillExcelSheets(string fileName)
    {
      Hashtable hashtable = new Hashtable();
      using (ExcelQueryFactory excelQueryFactory = new ExcelQueryFactory(fileName))
      {
        foreach (string worksheetName in excelQueryFactory.GetWorksheetNames())
        {
          RowNoHeader rowNoHeader = excelQueryFactory.WorksheetNoHeader(worksheetName).FirstOrDefault<RowNoHeader>((Expression<Func<RowNoHeader, bool>>) (r => (string) r[0] == "NN"));
          if (rowNoHeader != null)
          {
            IDictionary<string, int> dictionary = (IDictionary<string, int>) new Dictionary<string, int>();
            for (int index = 0; index < rowNoHeader.Capacity; ++index)
            {
              if (rowNoHeader[index].ToString() == "NN")
                dictionary.Add("Id", index);
              else if (rowNoHeader[index].ToString().ToUpper().Contains("ИМЯ"))
                dictionary.Add("Name", index);
              else if (rowNoHeader[index].ToString().ToUpper().Contains("ФАМИЛИЯ"))
                dictionary.Add("Surname", index);
              else if (rowNoHeader[index].ToString().ToUpper().Contains("КУРОРТ"))
                dictionary.Add("Resort", index);
              else if (rowNoHeader[index].ToString().ToUpper().Contains("ОТЕЛЬ"))
                dictionary.Add("HotelName", index);
            }
            hashtable.Add((object) worksheetName, (object) dictionary);
          }
        }
      }
      return hashtable;
    }

    private List<List<TouristTransferRow>> GetTourists(string fileName)
    {
      List<List<TouristTransferRow>> touristTransferRowListList = new List<List<TouristTransferRow>>();
      ExcelQueryFactory excelQueryFactory = new ExcelQueryFactory(fileName);
      Hashtable hashtable = this.FillExcelSheets(fileName);
      foreach (object key in (IEnumerable) hashtable.Keys)
      {
        bool flag = false;
        List<TouristTransferRow> touristTransferRowList = new List<TouristTransferRow>();
        foreach (RowNoHeader rowNoHeader in (QueryableBase<RowNoHeader>) excelQueryFactory.WorksheetNoHeader((string) key))
        {
          if (!((string) rowNoHeader[1] == string.Empty))
          {
            if (flag)
              touristTransferRowList.Add(this._rowCreator(rowNoHeader, (IDictionary<string, int>) hashtable[key]));
            else
              flag = true;
          }
        }
        touristTransferRowListList.Add(touristTransferRowList);
      }
      return touristTransferRowListList;
    }

    public ExcelTransferModel Parse(string fileName)
    {
      ExcelTransferModel excelTransferModel = new ExcelTransferModel();
      excelTransferModel.Type = PartnerType.Save;
      List<List<TouristTransferRow>> tourists = this.GetTourists(fileName);
      excelTransferModel.Tourists = tourists;
      return excelTransferModel;
    }
  }
}
