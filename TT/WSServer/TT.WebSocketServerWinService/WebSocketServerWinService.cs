using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using TT.Core.Logger;
using TT.DAL.Services;
using TT.WebSocketServer;

namespace TT.WSSWinService
{
    partial class WebSocketServerWinService : ServiceBase
    {
        private Server _WSServer;
        private IQuoteProvider _quoteProvider;
        public WebSocketServerWinService()
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
                    _WSServer = new Server();
                    _WSServer.Initialize();

                    _quoteProvider = new QuoteProvider(_WSServer);
                    Task.Run(() => _quoteProvider.Run());
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
                _WSServer?.Dispose();
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
