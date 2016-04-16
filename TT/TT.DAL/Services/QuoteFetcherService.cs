using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using RestSharp;
using TT.Core.Logger;
using TT.DAL.Pocos;
using TT.DAL.Rest;

namespace TT.DAL.Services
{
    public class QuoteFetcherService : IQuoteService
    {
         
        private const string URI = "http://widgets-m.fxpro.com/api/quotes";
        private AutoMapper.IMapper _mapper;

        public QuoteFetcherService()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<QuoteValueRest, QuotePoco>());
            _mapper = config.CreateMapper();
        }

        //http://widgets-m.fxpro.com/api/quotes?key=m!14ApI
        public IRestResponse<List<QuoteRest>> GetQuoteRests()
        {
            var client = new RestClient(URI);
            var request = new RestRequest(Method.GET);
            request.AddParameter("key", "m!14ApI");
            return client.Execute<List<QuoteRest>>(request);
        }

        public List<QuotePoco> GetQuotes()
        {
            var res = new List<QuotePoco>();
            var quoteRestsResponse = this.GetQuoteRests();
            if (quoteRestsResponse.StatusCode == HttpStatusCode.OK)
            {
                quoteRestsResponse.Data.ForEach(d => d.Values.ForEach(qValue => res.Add(_mapper.Map<QuotePoco>(qValue))));
            }
            else
            {
                Logger.Current.Error("unable to get dat from API " + URI, null);
                Logger.Current.Error(quoteRestsResponse.ErrorMessage, null);
            }
            return res;
        }
    }
}
