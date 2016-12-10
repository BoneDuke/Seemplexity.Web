// Decompiled with JetBrains decompiler
// Type: Seemplexity.Avalon.BusinesLogic.Services.ReservationCreationService
// Assembly: Seemplexity.Avalon.BusinesLogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 30BCE204-0124-42D8-B3AE-A0F92B55EC4D
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Avalon.BusinesLogic.dll

using Seemplexity.Common.Excel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace Seemplexity.Avalon.BusinesLogic.Services
{
  public class ReservationCreationService
  {
    private int CreateHotelService(Seemplexity.Avalon.BusinesLogic.Avalon context, int? hotelKey, DateTime turDate, string dgCode, int touristsCount, int tourKey)
    {
      ObjectParameter sResult = new ObjectParameter("sResult", typeof (string));
      ObjectParameter sResultLat = new ObjectParameter("sResultLat", typeof (string));
      ObjectParameter dTimeBeg = new ObjectParameter("dTimeBeg", typeof (DateTime));
      ObjectParameter dTimeEnd = new ObjectParameter("dTimeEnd", typeof (DateTime));
      context.MakeFullSVName(new int?(4), new int?(4), new int?(3), hotelKey, new int?(1), new int?(50744), new int?(73), new int?(5559), new DateTime?(turDate), string.Empty, sResult, sResultLat, dTimeBeg, dTimeEnd);
      string sService = sResult.Value.ToString();
      string sServiceLat = sResultLat.Value.ToString();
      ObjectParameter bRet = new ObjectParameter("bRet", typeof (int));
      ObjectParameter nPrmPlaceNoHave = new ObjectParameter("nPrmPlaceNoHave", typeof (Decimal));
      ObjectParameter dPrmBadDate = new ObjectParameter("dPrmBadDate", typeof (DateTime));
      ObjectParameter nNewKey = new ObjectParameter("nNewKey", typeof (int));
      context.InsDogList(dgCode, new DateTime?(DateTime.Now.Date), new int?(3), sService, new int?(1), hotelKey, new int?(50744), new int?(73), new int?(1), new int?(4), new int?(4), new int?(5559), new int?(), new int?(touristsCount), new Decimal?(new Decimal()), new Decimal?(new Decimal()), new Decimal?(new Decimal()), new int?(tourKey), new int?(tourKey), new int?(159), new int?(1), new int?(755), new int?(0), new DateTime?(turDate), new DateTime?(turDate), new DateTime?(turDate), new int?(0), bRet, nPrmPlaceNoHave, dPrmBadDate, nNewKey, new int?(), (string) null, (string) null, (string) null, (string) null, sServiceLat, new int?(), new int?(), new bool?());
      return (int) nNewKey.Value;
    }

    private int CreateExcursionService(Seemplexity.Avalon.BusinesLogic.Avalon context, int excursionKey, int countryKey, int cityKey, DateTime turDate, string dgCode, IList<tbl_Turist> tourists, int tourKey)
    {
      ObjectParameter sResult = new ObjectParameter("sResult", typeof (string));
      ObjectParameter sResultLat = new ObjectParameter("sResultLat", typeof (string));
      ObjectParameter dTimeBeg = new ObjectParameter("dTimeBeg", typeof (DateTime));
      ObjectParameter dTimeEnd = new ObjectParameter("dTimeEnd", typeof (DateTime));
      int num1 = 4;
      int num2 = 92;
      context.MakeFullSVName(new int?(countryKey), new int?(cityKey), new int?(num1), new int?(excursionKey), new int?(), new int?(num2), new int?(0), new int?(5559), new DateTime?(turDate), string.Empty, sResult, sResultLat, dTimeBeg, dTimeEnd);
      string sService = sResult.Value.ToString();
      string sServiceLat = sResultLat.Value.ToString();
      ObjectParameter bRet = new ObjectParameter("bRet", typeof (int));
      ObjectParameter nPrmPlaceNoHave = new ObjectParameter("nPrmPlaceNoHave", typeof (Decimal));
      ObjectParameter dPrmBadDate = new ObjectParameter("dPrmBadDate", typeof (DateTime));
      ObjectParameter nNewKey = new ObjectParameter("nNewKey", typeof (int));
      context.InsDogList(dgCode, new DateTime?(DateTime.Now.Date), new int?(num1), sService, new int?(1), new int?(excursionKey), new int?(num2), new int?(0), new int?(), new int?(countryKey), new int?(cityKey), new int?(5559), new int?(), new int?(tourists.Count), new Decimal?(new Decimal()), new Decimal?(new Decimal()), new Decimal?(new Decimal()), new int?(tourKey), new int?(tourKey), new int?(31), new int?(0), new int?(755), new int?(0), new DateTime?(turDate), new DateTime?(turDate), new DateTime?(turDate), new int?(0), bRet, nPrmPlaceNoHave, dPrmBadDate, nNewKey, new int?(), (string) null, (string) null, (string) null, (string) null, sServiceLat, new int?(), new int?(), new bool?());
      int num3 = (int) nNewKey.Value;
      foreach (tbl_Turist tourist in (IEnumerable<tbl_Turist>) tourists)
      {
        TuristService entity = new TuristService()
        {
          TU_TUKEY = tourist.TU_KEY,
          TU_DLKEY = num3,
          TU_NUMDOC = string.Empty
        };
        context.TuristServices.Add(entity);
      }
      return num3;
    }

    private int CreateTransferService(Seemplexity.Avalon.BusinesLogic.Avalon context, DateTime turDate, string dgCode, IList<tbl_Turist> tourists, int tourKey)
    {
      ObjectParameter sResult = new ObjectParameter("sResult", typeof (string));
      ObjectParameter sResultLat = new ObjectParameter("sResultLat", typeof (string));
      ObjectParameter dTimeBeg = new ObjectParameter("dTimeBeg", typeof (DateTime));
      ObjectParameter dTimeEnd = new ObjectParameter("dTimeEnd", typeof (DateTime));
      context.MakeFullSVName(new int?(4), new int?(9), new int?(2), new int?(1455), new int?(), new int?(77), new int?(0), new int?(5559), new DateTime?(turDate), string.Empty, sResult, sResultLat, dTimeBeg, dTimeEnd);
      string sService = sResult.Value.ToString();
      string sServiceLat = sResultLat.Value.ToString();
      ObjectParameter bRet = new ObjectParameter("bRet", typeof (int));
      ObjectParameter nPrmPlaceNoHave = new ObjectParameter("nPrmPlaceNoHave", typeof (Decimal));
      ObjectParameter dPrmBadDate = new ObjectParameter("dPrmBadDate", typeof (DateTime));
      ObjectParameter nNewKey = new ObjectParameter("nNewKey", typeof (int));
      context.InsDogList(dgCode, new DateTime?(DateTime.Now.Date), new int?(2), sService, new int?(1), new int?(1455), new int?(77), new int?(0), new int?(), new int?(4), new int?(9), new int?(5559), new int?(), new int?(tourists.Count), new Decimal?(new Decimal()), new Decimal?(new Decimal()), new Decimal?(new Decimal()), new int?(tourKey), new int?(tourKey), new int?(31), new int?(0), new int?(755), new int?(0), new DateTime?(turDate), new DateTime?(turDate), new DateTime?(turDate), new int?(0), bRet, nPrmPlaceNoHave, dPrmBadDate, nNewKey, new int?(), (string) null, (string) null, (string) null, (string) null, sServiceLat, new int?(), new int?(), new bool?());
      int num = (int) nNewKey.Value;
      foreach (tbl_Turist tourist in (IEnumerable<tbl_Turist>) tourists)
      {
        TuristService entity = new TuristService()
        {
          TU_TUKEY = tourist.TU_KEY,
          TU_DLKEY = num,
          TU_NUMDOC = string.Empty
        };
        context.TuristServices.Add(entity);
      }
      return num;
    }

    public List<string> SaveExcelModel(ExcelTransferModel model)
    {
      List<string> stringList = new List<string>();
      using (Seemplexity.Avalon.BusinesLogic.Avalon context = new Seemplexity.Avalon.BusinesLogic.Avalon())
      {
        foreach (List<TouristTransferRow> tourist in model.Tourists)
        {
          DateTime date = model.TourDate.Date;
          ObjectParameter name = new ObjectParameter("name", typeof (string));
          context.MakePutName(new DateTime?(date), new int?(4), new int?(), new int?(17787), new int?(0), "PCYMMDD999", name);
          string str1 = name.Value.ToString();
          stringList.Add(str1);
          int tourKey = 17787;
          ObjectParameter nReturn = new ObjectParameter("nReturn", typeof (int));
          ObjectParameter nKeyDogovor = new ObjectParameter("nKeyDogovor", typeof (int));
          context.InsDogovor(nReturn, nKeyDogovor, str1, new DateTime?(date), new int?(2), new int?(tourKey), new int?(4), new int?(4), new short?((short) tourist.Count), "EU", new Decimal?(new Decimal()), new Decimal?(new Decimal()), new Decimal?(new Decimal()), new int?(), new short?((short) 0), new Decimal?(new Decimal()), new int?(), new int?(0), new int?(), (string) null, (string) null, (string) null, (string) null, (string) null, new int?(755), new int?(1), new short?((short) 0), new short?((short) 0), new int?(), new short?(), new DateTime?(), new DateTime?(), new DateTime?(), new int?(), new int?(), (string) null, new int?(0), (string) null, (string) null, new int?(), new int?(), new int?(), new int?(), new int?(), (string) null);
          int num1 = (int) nKeyDogovor.Value;
          List<tbl_Turist> tblTuristList = new List<tbl_Turist>();
          List<TuristService> turistServiceList = new List<TuristService>();
          foreach (int? nullable1 in tourist.Where<TouristTransferRow>((Func<TouristTransferRow, bool>) (t =>
          {
            if (t.AvalonHotelKey.HasValue)
              return t.AvalonHotelKey.Value > 0;
            return false;
          })).Select<TouristTransferRow, int?>((Func<TouristTransferRow, int?>) (t => t.AvalonHotelKey)).Distinct<int?>().ToList<int?>())
          {
            int? hotelKey = nullable1;
            List<TouristTransferRow> list = tourist.Where<TouristTransferRow>((Func<TouristTransferRow, bool>) (t =>
            {
              int? avalonHotelKey = t.AvalonHotelKey;
              int? nullable = hotelKey;
              if (avalonHotelKey.GetValueOrDefault() != nullable.GetValueOrDefault())
                return false;
              return avalonHotelKey.HasValue == nullable.HasValue;
            })).ToList<TouristTransferRow>();
            int hotelService = this.CreateHotelService(context, hotelKey, date, str1, list.Count, tourKey);
            ObjectParameter nNewKey = new ObjectParameter("nNewKey", typeof (int));
            context.GetNewKeys("Turist", new int?(list.Count), nNewKey);
            int num2 = (int) nNewKey.Value;
            foreach (TouristTransferRow touristTransferRow in list)
            {
              string str2 = touristTransferRow.Name.Trim();
              string str3 = str2.Substring(0, Math.Min(15, str2.Length));
              string str4 = touristTransferRow.Surname.Trim();
              string str5 = str4.Substring(0, Math.Min(35, str4.Length));
              tbl_Turist tblTurist1 = new tbl_Turist();
              tblTurist1.TU_KEY = num2;
              tblTurist1.TU_DGCOD = str1;
              DateTime? nullable2 = new DateTime?(date);
              tblTurist1.TU_TURDATE = nullable2;
              string str6 = str3;
              tblTurist1.TU_FNAMERUS = str6;
              string str7 = str3;
              tblTurist1.TU_FNAMELAT = str7;
              string str8 = str5;
              tblTurist1.TU_NAMERUS = str8;
              string str9 = str5;
              tblTurist1.TU_NAMELAT = str9;
              string str10 = str5[0].ToString() + ".";
              tblTurist1.TU_SHORTNAME = str10;
              int num3 = 755;
              tblTurist1.TU_CREATOR = num3;
              int num4 = 755;
              tblTurist1.TU_OWNER = num4;
              int? nullable3 = new int?(num1);
              tblTurist1.TU_DGKEY = nullable3;
              tbl_Turist tblTurist2 = tblTurist1;
              tblTuristList.Add(tblTurist2);
              TuristService turistService = new TuristService()
              {
                TU_TUKEY = num2,
                TU_DLKEY = hotelService,
                TU_NUMDOC = string.Empty
              };
              turistServiceList.Add(turistService);
              --num2;
            }
          }
          context.TuristServices.AddRange((IEnumerable<TuristService>) turistServiceList);
          context.tbl_Turist.AddRange((IEnumerable<tbl_Turist>) tblTuristList);
          this.CreateTransferService(context, date, str1, (IList<tbl_Turist>) tblTuristList, tourKey);
          context.SaveChanges();
        }
      }
      return stringList;
    }

    public List<string> SaveExcelModel(ExcelExcursionModel model)
    {
      List<string> stringList = new List<string>();
      var list1 = model.Tourists.Select(t => new
      {
        Date = t.Date,
        AvalonHotelKey = t.AvalonHotelKey,
        AvalonExcursionKey = t.AvalonExcursionKey
      }).Distinct().ToList();
      using (Seemplexity.Avalon.BusinesLogic.Avalon context = new Seemplexity.Avalon.BusinesLogic.Avalon())
      {
        foreach (var data in list1)
        {
          var exc = data;
          List<TouristItem> list2 = model.Tourists.Where<TouristExcursionRow>((Func<TouristExcursionRow, bool>) (t =>
          {
            if (t.Date == exc.Date)
            {
              int? nullable1 = t.AvalonExcursionKey;
              int? nullable2 = exc.AvalonExcursionKey;
              if ((nullable1.GetValueOrDefault() == nullable2.GetValueOrDefault() ? (nullable1.HasValue == nullable2.HasValue ? 1 : 0) : 0) != 0)
              {
                nullable2 = t.AvalonHotelKey;
                nullable1 = exc.AvalonHotelKey;
                if (nullable2.GetValueOrDefault() != nullable1.GetValueOrDefault())
                  return false;
                return nullable2.HasValue == nullable1.HasValue;
              }
            }
            return false;
          })).ToList<TouristExcursionRow>().SelectMany<TouristExcursionRow, TouristItem>((Func<TouristExcursionRow, IEnumerable<TouristItem>>) (r => (IEnumerable<TouristItem>) r.Tourists)).ToList<TouristItem>();
          int tourKey = 17787;
          DateTime date = exc.Date;
          ObjectParameter name = new ObjectParameter("name", typeof (string));
          context.MakePutName(new DateTime?(date), new int?(4), new int?(), new int?(tourKey), new int?(0), "PCYMMDD999", name);
          string str1 = name.Value.ToString();
          stringList.Add(str1);
          ObjectParameter nReturn = new ObjectParameter("nReturn", typeof (int));
          ObjectParameter nKeyDogovor = new ObjectParameter("nKeyDogovor", typeof (int));
          context.InsDogovor(nReturn, nKeyDogovor, str1, new DateTime?(date), new int?(2), new int?(tourKey), new int?(4), new int?(4), new short?((short) list2.Count), "EU", new Decimal?(new Decimal()), new Decimal?(new Decimal()), new Decimal?(new Decimal()), new int?(), new short?((short) 0), new Decimal?(new Decimal()), new int?(), new int?(0), new int?(), (string) null, (string) null, (string) null, (string) null, (string) null, new int?(755), new int?(1), new short?((short) 0), new short?((short) 0), new int?(), new short?(), new DateTime?(), new DateTime?(), new DateTime?(), new int?(), new int?(), (string) null, new int?(0), (string) null, (string) null, new int?(), new int?(), new int?(), new int?(), new int?(), (string) null);
          int num1 = (int) nKeyDogovor.Value;
          List<tbl_Turist> tblTuristList = new List<tbl_Turist>();
          List<TuristService> turistServiceList = new List<TuristService>();
          int? avalonHotelKey = exc.AvalonHotelKey;
          int hotelService = this.CreateHotelService(context, avalonHotelKey, date, str1, list2.Count, tourKey);
          ObjectParameter nNewKey = new ObjectParameter("nNewKey", typeof (int));
          context.GetNewKeys("Turist", new int?(list2.Count), nNewKey);
          int num2 = (int) nNewKey.Value;
          foreach (TouristItem touristItem in list2)
          {
            tbl_Turist tblTurist1 = new tbl_Turist();
            tblTurist1.TU_KEY = num2;
            tblTurist1.TU_DGCOD = str1;
            DateTime? nullable1 = new DateTime?(date);
            tblTurist1.TU_TURDATE = nullable1;
            string str2 = touristItem.Name.Trim();
            tblTurist1.TU_FNAMERUS = str2;
            string str3 = touristItem.Name.Trim();
            tblTurist1.TU_FNAMELAT = str3;
            string str4 = touristItem.Surname.Trim();
            tblTurist1.TU_NAMERUS = str4;
            string str5 = touristItem.Surname.Trim();
            tblTurist1.TU_NAMELAT = str5;
            string str6 = touristItem.Surname[0].ToString() + ".";
            tblTurist1.TU_SHORTNAME = str6;
            int num3 = 755;
            tblTurist1.TU_CREATOR = num3;
            int num4 = 755;
            tblTurist1.TU_OWNER = num4;
            int? nullable2 = new int?(num1);
            tblTurist1.TU_DGKEY = nullable2;
            tbl_Turist tblTurist2 = tblTurist1;
            tblTuristList.Add(tblTurist2);
            TuristService turistService = new TuristService()
            {
              TU_TUKEY = num2,
              TU_DLKEY = hotelService,
              TU_NUMDOC = string.Empty
            };
            turistServiceList.Add(turistService);
            --num2;
          }
          context.TuristServices.AddRange((IEnumerable<TuristService>) turistServiceList);
          context.tbl_Turist.AddRange((IEnumerable<tbl_Turist>) tblTuristList);
          ExcurDictionary excurDictionary = context.ExcurDictionaries.FirstOrDefault<ExcurDictionary>((Expression<Func<ExcurDictionary, bool>>) (e => (int?) e.ED_KEY == exc.AvalonExcursionKey));
          if (excurDictionary == null)
            throw new Exception("На нейдена экскурсия");
          this.CreateExcursionService(context, excurDictionary.ED_KEY, excurDictionary.ED_CNKEY, excurDictionary.ED_CTKEY, date, str1, (IList<tbl_Turist>) tblTuristList, tourKey);
          context.SaveChanges();
        }
      }
      return stringList;
    }
  }
}
