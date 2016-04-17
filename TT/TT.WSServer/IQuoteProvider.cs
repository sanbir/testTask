
using System.Collections.Generic;
using TT.DAL.Pocos;

namespace TT.WSServer
{
    public interface IQuoteProvider
    {
        void Provide(List<QuotePoco> quotes);
        void Run();

    }
}
