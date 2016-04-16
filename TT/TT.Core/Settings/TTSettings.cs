using System;
using System.Runtime.CompilerServices;

namespace TT.Core.Settings
{
    [CompilerGenerated]
    public static class TTSettings
    {
        public static string MongoConnectionString
        {
            get { return Get("MongoConnectionString"); }
        }

        private static string Get(string settingName)
        {
            return SharedConfiguration.Instance.AppSettings.Settings[settingName].Value;
        }

        private static int GetInt(string settingName)
        {
            return Convert.ToInt32(SharedConfiguration.Instance.AppSettings.Settings[settingName].Value);
        }

        private static bool GetBoolean(string settingName)
        {
            return Convert.ToBoolean(SharedConfiguration.Instance.AppSettings.Settings[settingName].Value);
        }
    }
}