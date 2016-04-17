using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TT.DAL.Pocos;
using TT.DAL.Repository;
using TT.DAL.Services;

namespace TT.WSServer
{
    public class QuoteProvider : IQuoteProvider
    {
        private readonly IQuoteService _quoteService;
        private readonly Server _webSocketServer;
        private readonly IQuoteRepository _quoteRepository;
        

        public QuoteProvider(IQuoteService quoteService, Server webSocketServer, IQuoteRepository quoteRepository )
        {
            _quoteService = quoteService;
            _webSocketServer = webSocketServer;
            _quoteRepository = quoteRepository;
        }

        public QuoteProvider(Server server) : this(new QuoteService(), server, new QuoteRepository())
        {
        }

        private void AddNewToDatabase(List<QuotePoco> quotes)
        {
            _quoteRepository.Add(quotes);
        }

        private void UpdateDatabase(List<QuotePoco> quotes)
        {
            Task.Run(() => AddNewToDatabase(quotes));
            Task.Run(() => RemoveOldFromDatabase());
        }
        

        public void Run()
        {
            while (true)
            {
                Thread.Sleep(5000);

                var quotes = _quoteService.GetQuotes();

                if(quotes.Count > 0)
                Provide(quotes);
            }
        }

        public void Provide(List<QuotePoco> quotes )
        {
           Task.Run(() => _webSocketServer.ProcessQuotes(quotes));
           Task.Run(() => UpdateDatabase(quotes));
        }

        private void RemoveOldFromDatabase()
        {
            var hour = new TimeSpan(0, 1, 0, 0);

            _quoteRepository.RemoveUntil(DateTime.UtcNow - hour);
        }

        

        
    }
}
