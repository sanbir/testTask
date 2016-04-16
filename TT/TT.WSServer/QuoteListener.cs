using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TT.DAL.Pocos;
using TT.DAL.Services;

namespace TT.WSServer
{
    public class QuoteListener:IQuoteListener
    {
        private readonly Server _webSocketServer;
        private readonly IQuoteService _quoteService;

        public QuoteListener(Server webSocketServer)
        {
            _webSocketServer = webSocketServer;
            _quoteService = new QuoteFetcherService();
        }

        private List<QuotePoco> _previousQuotes; 

        public void Listen()
        {

            while (true)
            {
                Thread.Sleep(5000);
                
                var quotes = _quoteService.GetQuotes();

                var quotesToSend = new List<QuotePoco>();

                foreach (var quote in quotes)
                {
                    var found = _previousQuotes.FirstOrDefault(q => q.SymbolName == quote.SymbolName);
                    if (found != null)
                    {
                        if (!AreEqual(found, quote))
                        {
                            quotesToSend.Add(quote);
                        }
                    }
                    else
                    {
                        quotesToSend.Add(quote);
                    }
                }

                _webSocketServer.NotifySubscribers(quotesToSend);
            }
        }

        private bool AreEqual(QuotePoco quote, QuotePoco quote2)
        {
            return quote.AskValue == quote2.AskValue && quote.BidValue == quote2.BidValue;
        }
    }
}
