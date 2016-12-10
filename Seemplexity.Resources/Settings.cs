using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seemplexity.Resources
{
    public static class Settings
    {
        public static List<Language> Languages = new List<Language>
            {
            new Language() {Code = "ru", FlagCode = "ru", Name = "RUS", RateCode = "EU"},
            new Language() {Code = "en", FlagCode = "gb", Name = "ENG", RateCode = "EU"},
            new Language() {Code = "ro", FlagCode = "ro", Name = "ROM", RateCode = "L"},
            new Language() {Code = "bg", FlagCode = "bg", Name = "BUL", RateCode = "EU"}
            };

        public static string DefaultLanguage = "en";

        public static List<Rate> Rates = new List<Rate>
        {
            new Rate() {Code = "RUB", Name = "рб"},
            new Rate() {Code = "USD", Name = "$"},
            new Rate() {Code = "EU", Name = "EU"},
            new Rate() {Code = "L", Name = "Leu"}
        };
    }
}
