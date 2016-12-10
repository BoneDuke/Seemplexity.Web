using System.Threading;

namespace Seemplexity.Common
{
    public class Name
    {
        public string NameRu { get; set; }
        public string NameEn { get; set; }

        public Name(string nameRu, string nameEn)
        {
            NameRu = nameRu;
            NameEn = nameEn;
        }

        public string Actual => Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName == "ru" ? NameRu : NameEn;
    }
}
