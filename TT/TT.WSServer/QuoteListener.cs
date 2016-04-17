using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TT.DAL.Pocos;
using TT.DAL.Repository;
using TT.DAL.Services;

namespace TT.WSServer
{
    public class QuoteListener : IQuoteListener
    {
        private readonly Server _webSocketServer;

        private readonly IQuoteService _quoteService;
        private readonly IQuoteRepository _quoteRepository;

        public QuoteListener(Server webSocketServer)
            : this(webSocketServer, new QuoteFetcherService(), new QuoteRepository())
        {
        }

        public QuoteListener(Server webSocketServer, IQuoteService quoteService, IQuoteRepository quoteRepository)
        {
            _webSocketServer = webSocketServer;
            _quoteService = quoteService;
            _quoteRepository = quoteRepository;
        }

        private List<QuotePoco> _previousQuotes = new List<QuotePoco>(); 

        public void Listen()
        {
            while (true)
            {
                Thread.Sleep(5000);

                var quotes = _quoteService.GetQuotes();

                Task.Run(() => NotifySubscribers(quotes));
                Task.Run(() => UpdateDatabase(quotes));
            }
        }

        private void UpdateDatabase(List<QuotePoco> quotes)
        {
            _quoteRepository.Add(quotes);
        }

        private void NotifySubscribers(List<QuotePoco> quotes)
        {
            if (WSServer.Server.ClientInfo.Values.Count < 1)
            {
                return;
            }

            var quotesToSend = new List<QuotePoco>();

            foreach (var quote in quotes)
            {
                var found = _previousQuotes.FirstOrDefault(q => q.Symbol == quote.Symbol);
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

            _previousQuotes = quotes;

            _webSocketServer.NotifySubscribers(quotesToSend);
        }

        private bool AreEqual(QuotePoco quote, QuotePoco quote2)
        {
            return quote.Ask == quote2.Ask && quote.Bid == quote2.Bid;
        }
    }
}
