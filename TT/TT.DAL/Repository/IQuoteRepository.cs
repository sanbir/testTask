using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TT.DAL.Pocos;

namespace TT.DAL.Repository
{
    public interface IQuoteRepository
    {
        void Add(IEnumerable<QuotePoco> quotes);

        void RemoveUntil(DateTime time);

        List<QuotePoco> Get(string symbol);
    }
}
