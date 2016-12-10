// Decompiled with JetBrains decompiler
// Type: Seemplexity.Common.Services.ExcelModelService
// Assembly: Seemplexity.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 890F1AE1-8FE0-4739-8116-070B8BB405DC
// Assembly location: C:\Programming\Balkan\bin\Seemplexity.Common.dll

using Seemplexity.Common.Excel;
using Seemplexity.Common.Helpers.Excel;
using System;

namespace Seemplexity.Common.Services
{
  public class ExcelModelService
  {
    public ExcelExcursionModel GetExcursionModelFromFile(string fileName)
    {
      return new ExcursionParser().Parse(fileName);
    }

    public ExcelTransferModel GetTransferModelFromFile(string fileName)
    {
      ITransferParser transferParser;
      switch (Utils.GetPartnerTypeByFileName(fileName))
      {
        case PartnerType.Gloria:
          transferParser = (ITransferParser) new GloriaParser();
          break;
        case PartnerType.Mari:
          transferParser = (ITransferParser) new MariParser();
          break;
        case PartnerType.Save:
          transferParser = (ITransferParser) new SaveParser();
          break;
        case PartnerType.Vision:
          transferParser = (ITransferParser) new VisionParser();
          break;
        case PartnerType.Word:
          transferParser = (ITransferParser) new WordParser();
          break;
        default:
          throw new NotSupportedException("Не распознанный тип файла");
      }
      ExcelTransferModel excelTransferModel = transferParser.Parse(fileName);
      DateTime date = DateTime.Now.Date;
      excelTransferModel.TourDate = date;
      return excelTransferModel;
    }
  }
}
