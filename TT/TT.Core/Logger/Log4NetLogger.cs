using System;
using System.IO;
using log4net;
using TT.Core.Settings;

namespace TT.Core.Logger
{
    public class Log4NetLogger : ILogger
    {
        private ILog instance;
        public Log4NetLogger(string logName = "default", bool useSharedConfig = false)
        {
            this.instance = LogManager.GetLogger(logName);

            if(useSharedConfig)
            log4net.Config.XmlConfigurator.Configure(new FileInfo(SharedConfiguration.Instance.FilePath));
            else
            {
                log4net.Config.XmlConfigurator.Configure();
            }
        }

        public void Info(string message)
        {
            this.instance.Info(message);
        }

        public void Warning(string message, Exception e = null)
        {
            this.instance.Warn(message, e);
        }

        public void Error(string message, Exception e = null)
        {
            this.instance.Error(message, e);
        }

        public void ErrorFormat(string format, object arg0)
        {
            instance.ErrorFormat(format, arg0);
        }
    }
}
