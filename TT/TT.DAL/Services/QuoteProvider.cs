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
        private readonly IQuoteSubscriber _webSocketServer;
        
        

        public QuoteProvider(IQuoteService quoteService, IQuoteSubscriber webSocketServer )
        {
            _quoteService = quoteService;
            _webSocketServer = webSocketServer;
        }

        public QuoteProvider(IQuoteSubscriber server) : this(new QuoteService(), server)
        {
        }

        
        

        public void Run()
        {
            while (true)
            {
                var quotes = _quoteService.GetQuotes();

                if(quotes.Count > 0)
                Provide(quotes);

                Thread.Sleep(5000);
            }
        }

        public void Provide(List<QuotePoco> quotes )
        {
           Task.Run(() => _webSocketServer.ProcessQuotes(quotes));
        }

        

        

        
    }
}
