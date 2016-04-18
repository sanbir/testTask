using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TT.Core.Logger;
using TT.DAL.Services;
using TT.WSServer;

namespace TT.WebSocketPublisher
{
    partial class PublisherWinService : ServiceBase
    {
        private static Server _fleckServer;

        public PublisherWinService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += CurrentDomain_UnhandledException;

            try
            {
                Task.Run(() =>
                {
                    _fleckServer = new Server();
                    _fleckServer.Initialize();
                }
                    );
                Logger.Current.Info("--SERVICE STARTED--");


                //console mode:
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex.Message, ex);
                throw;
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Logger.Current.Error($"CurrentDomain_UnhandledException: {ex.Message} terminating: {e.IsTerminating}", ex);
        }

        protected override void OnStop()
        {
            try
            {
                _fleckServer?.Dispose();
                Logger.Current.Info("--SERVICE STOPPED--");
            }
            catch (Exception ex)
            {
                Logger.Current.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
