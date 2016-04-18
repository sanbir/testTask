﻿using System;
using System.ServiceProcess;
using System.Threading.Tasks;
using TT.Core.Logger;
using TT.DAL.Services;
using TT.WSServer;

namespace TT.MarketDataService
{
    partial class PublisherWinService : ServiceBase
    {
        private MarketDataUpdater.MarketDataUpdater _marketDataUpdater;
        private IQuoteProvider _quoteProvider;
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
                    _marketDataUpdater = new MarketDataUpdater.MarketDataUpdater();
                    
                    _quoteProvider = new QuoteProvider(_marketDataUpdater);
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
