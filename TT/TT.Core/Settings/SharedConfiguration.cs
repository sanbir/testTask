using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace TT.Core.Settings
{
    public class SharedConfiguration
    {
        public static readonly Configuration Instance = GetConfiguration("../../../TT.Core/shared.config");

        private static Configuration GetConfiguration(string configFileName)
        {
            try
            {
                var exeConfigurationFileMap = new ExeConfigurationFileMap();
                var uri = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase));
                exeConfigurationFileMap.ExeConfigFilename = Path.Combine(uri.LocalPath, configFileName);
                return ConfigurationManager.OpenMappedExeConfiguration(exeConfigurationFileMap, ConfigurationUserLevel.None);
            }
            catch (Exception e)
            {
                Logger.Logger.Current.Error("loading config: " + e, e);
                throw new Exception("Can't load app shared configuration");
            }
        }
    }
}