using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TT.DAL.Pocos;

namespace TT.DAL.Services
{
    public class QuoteProvider : IQuoteProvider
    {
        private readonly IQuoteService _quoteService;
        private readonly IQuoteSubscriber _quoteSubscriber;
        

        public QuoteProvider(IQuoteService quoteService, IQuoteSubscriber quoteSubscriber)
        {
            _quoteService = quoteService;
            _quoteSubscriber = quoteSubscriber;
        }

        public QuoteProvider(IQuoteSubscriber quoteSubscriber) : this(new QuoteService(), quoteSubscriber)
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
           Task.Run(() => _quoteSubscriber.ProcessQuotes(quotes));
        }
        
    }
}
