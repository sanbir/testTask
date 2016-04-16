using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using TT.DAL.Rest;

namespace TT.DAL.Services
{
    public interface IQuoteService
    {
        IRestResponse<List<QuoteRest>> GetQuoutes();
    }
}
