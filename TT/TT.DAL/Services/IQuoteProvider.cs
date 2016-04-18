using System.Collections.Generic;
using TT.DAL.Pocos;

namespace TT.DAL.Services
{
    public interface IQuoteProvider
    {
        void Provide(List<QuotePoco> quotes);
        void Run();

    }

    public interface IQuoteSubscriber
    {
        void ProcessQuotes(List<QuotePoco> quotes);
      
    }
}
