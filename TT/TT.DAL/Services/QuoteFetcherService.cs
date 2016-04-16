using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using TT.DAL.Rest;

namespace TT.DAL.Services
{
    public class QuoteFetcherService : IQuoteService
    {
        private const string URI = "http://widgets-m.fxpro.com/api";

        //http://widgets-m.fxpro.com/api/quotes?key=m!14ApI
        public IRestResponse<List<QuoteRest>> GetQuoutes()
        {
            var client = new RestClient(URI);
            var request = new RestRequest(Method.GET);
            request.AddParameter("key", "m!14ApI");
            return client.Execute<List<QuoteRest>>(request);
        }
    }
}
