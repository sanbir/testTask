using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Core.Logger;
using TT.DAL.Pocos;
using TT.DAL.Repository;
using TT.DAL.Services;

namespace TT.MarketDataUpdater
{
    public class MarketDataUpdater: IQuoteSubscriber
    {
        private readonly IQuoteRepository _quoteRepository;
        public MarketDataUpdater(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public MarketDataUpdater(): this(new QuoteRepository())
        {
          
        }
        public void ProcessQuotes(List<QuotePoco> quotes)
        {
            UpdateDatabase(quotes);
        }
        private void UpdateDatabase(List<QuotePoco> quotes)
        {
            Task.Run(() => AddNewToDatabase(quotes));
            Task.Run(() => RemoveOldFromDatabase());
        }

        private void AddNewToDatabase(List<QuotePoco> quotes)
        {
            _quoteRepository.Add(quotes);

            Logger.Current.Info("New quotes added");
        }

        private void RemoveOldFromDatabase()
        {
            var hour = new TimeSpan(0, 1, 0, 0);

            _quoteRepository.RemoveUntil(DateTime.UtcNow - hour);

            Logger.Current.Info("Old quotes deleted");
        }

        
    }
}
