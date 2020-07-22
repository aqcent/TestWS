using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TestWS
{
    public static class Constants
    {
        public static NameValueCollection UrlsToWarmUp => ConfigurationManager.GetSection("CacheWarmingUpUrls") as NameValueCollection;
        public static string Domain => ConfigurationManager.AppSettings["Domain"];

        public static string ExcelTemplatesDirectory => ConfigurationManager.AppSettings["ExcelTemplatesDirectory"] ;
        public static string ReportsDirectory => ConfigurationManager.AppSettings["ReportsDirectory"] ;
        public static TimeSpan DefaultCacheTime => GetMinutesTimeSpanSetting("DefaultCacheTime", 30);

        private static TimeSpan GetMinutesTimeSpanSetting(string settingName, int defaultValue)
        {
            var settingValue = ConfigurationManager.AppSettings[settingName];

            if (int.TryParse(settingValue, out int parsedValue))
                return TimeSpan.FromMinutes(parsedValue);

            return TimeSpan.FromMinutes(defaultValue);
        }
    }
}