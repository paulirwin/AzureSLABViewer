using Microsoft.WindowsAzure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureSLABViewer.Web
{
    public static class ConfigSettings
    {
        static ConfigSettings()
        {
            AllowRegistration = GetValueOrDefault("AllowRegistration", true);
        }

        private static bool GetValueOrDefault(string key, bool defaultValue)
        {
            string value = CloudConfigurationManager.GetSetting(key);
            bool b;

            if (!bool.TryParse(value, out b))
                return defaultValue;

            return b;
        }

        private static string GetValueOrDefault(string key, string defaultValue)
        {
            string value = CloudConfigurationManager.GetSetting(key);

            if (string.IsNullOrEmpty(value))
                return defaultValue;

            return value;
        }

        public static bool AllowRegistration { get; private set; }
    }
}