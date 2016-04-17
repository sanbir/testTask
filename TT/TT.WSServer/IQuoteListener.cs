using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.DAL.Pocos;

namespace TT.WSServer
{
    public interface IQuoteListener
    {
        void Listen();
        List<QuotePoco> PreviousQuotes { get; } 
    }
}
