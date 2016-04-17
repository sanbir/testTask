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
    public class QuoteListener : IQuoteListener
    {
        private readonly Server _webSocketServer;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IQuoteService _quoteService;

        public QuoteListener(Server webSocketServer, IQuoteRepository quoteRepository, IQuoteService quoteService)
        {
            _webSocketServer = webSocketServer;
            _quoteRepository = quoteRepository;
            _quoteService = quoteService;

            PreviousQuotes = _quoteService.GetQuotes();
        }

        public QuoteListener(Server server) : this(server, new QuoteRepository(), new QuoteFetcherService())
        {
        }

        private void AddNewToDatabase(List<QuotePoco> quotes)
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
            if (quotesToSend.Count > 0)
                _webSocketServer.NotifySubscribers(quotesToSend);
        }

        public void Listen()
        {
            while (true)
            {
                Thread.Sleep(5000);

                var quotes = _quoteService.GetQuotes();

                Task.Run(() => NotifySubscribers(quotes));
                Task.Run(() => AddNewToDatabase(quotes));
                Task.Run(() => RemoveOldFromDatabase());
            }
        }

        private void RemoveOldFromDatabase()
        {
            var hour = new TimeSpan(0, 1, 0, 0);

            _quoteRepository.RemoveUntil(DateTime.UtcNow - hour);
        }

        public List<QuotePoco> PreviousQuotes { get; private set; }

        private bool AreEqual(QuotePoco quote, QuotePoco quote2)
        {
            return quote.Ask == quote2.Ask && quote.Bid == quote2.Bid;
        }
    }
}
