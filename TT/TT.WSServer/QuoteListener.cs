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

            PreviousQuotes = _quoteService.GetQuotes();
        }

        

        public void Listen()
        {

            while (true)
            {
                Thread.Sleep(5000);

                if (WSServer.Server.ClientInfo.Values.Count < 1)
                {
                    continue;
                }

                var quotes = _quoteService.GetQuotes();

                var quotesToSend = new List<QuotePoco>();

                foreach (var quote in quotes)
                {
                    var found = PreviousQuotes.FirstOrDefault(q => q.Symbol == quote.Symbol);
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

                PreviousQuotes = quotes;
                if(quotesToSend.Count > 0)
                _webSocketServer.NotifySubscribers(quotesToSend);
            }
        }

        public List<QuotePoco> PreviousQuotes { get; private set;}

        private bool AreEqual(QuotePoco quote, QuotePoco quote2)
        {
            return quote.Ask == quote2.Ask && quote.Bid == quote2.Bid;
        }
    }
}
