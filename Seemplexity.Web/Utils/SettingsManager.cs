using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Seemplexity.BusinesLogic.Model;

namespace Seemplexity.Web.Utils
{
    public static class SettingsManager
    {
        public const string BusTourType = "BusTourType";
        public const string BusServiceKey = "BusServiceKey";

        public static IList<int> GetSettingListValue(string settingName)
        {
            return ApplicationSettings[settingName].Split(',').Select(int.Parse).ToList();
        }

        private static Dictionary<string, string> _applicationSettings;
        public static Dictionary<string, string> ApplicationSettings
        {
            get
            {
                if (_applicationSettings == null)
                {
                    using (var context = new SeemplexityModel())
                    {
                        _applicationSettings = context.Settings.ToDictionary(s => s.Name, s => s.Value);
                    }
                }
                return _applicationSettings;
            }
        }
    }
}