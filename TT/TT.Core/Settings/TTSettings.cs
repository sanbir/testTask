using System;
using System.Configuration;
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

        /// <summary>
        /// Gets a value from the AppSettings config section, casted to type T
        /// </summary>
        /// <typeparam name="T">Type to cast value to</typeparam>
        /// <param name="key">AppSetting key</param>
        /// <param name="defaultVal">Default value on error</param>
        /// <param name="throwOnException">Whether to throw an exception on error</param>
        /// <param name="warnOnMissingFromConfig">Whether to log warn if missing from config. This should ONLY be set
        /// to false if you don't plan on putting the setting in the config, but want to allow adding the setting to
        /// config if need be. *This parameter is not used if throwOnException is true.</param>
        /// <returns></returns>
        public static T GetAppSetting<T>(string key, T defaultVal = default(T), bool throwOnException = false, bool warnOnMissingFromConfig = true)
        {
            T val;
            try
            {
                var reader = new AppSettingsReader();
                val = (T)reader.GetValue(key, typeof(T));
            }
            catch (ArgumentNullException) // Key or T is null
            {
                if (throwOnException)
                    throw;
                if (warnOnMissingFromConfig)
                    Logger.Logger.Current.Warning($"App setting '{key}' is null; using default: {defaultVal}");
                val = defaultVal;
            }
            catch (InvalidOperationException ex) // Value could not be parsed to type T
            {
                if (throwOnException)
                    throw;
                if (ex.Message.Contains("does not exist in the appSettings configuration section"))
                {
                    if (warnOnMissingFromConfig)
                        Logger.Logger.Current.Warning(
                            $"App setting '{key}' does not exist in the appSettings configuration section; using default: {defaultVal}");
                }
                else
                {
                    Logger.Logger.Current.Error($"Using default for AppSetting '{key}': {defaultVal}; {ex.Message}");
                }
                val = defaultVal;
            }
            return val;
        }
    }
}