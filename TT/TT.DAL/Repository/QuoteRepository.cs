using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MongoDB.Driver.Linq;
using MongoRepository;
using TT.DAL.Entity;
using TT.DAL.Pocos;
using TT.DAL.Rest;

namespace TT.DAL.Repository
{
    public class QuoteRepository : BaseMongoRepository<QuoteEntity>, IQuoteRepository
    {
        private static int _maximumPointsNumber = 720;
        private AutoMapper.IMapper _mapper;

        public QuoteRepository() : base()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<QuotePoco, QuoteEntity>());
            _mapper = config.CreateMapper();
        }

        public void Add(IEnumerable<QuotePoco> quotes)
        {
            foreach (var quote in quotes)
            {
                var quouteToDb = _mapper.Map<QuoteEntity>(quote);
                this.Collection.Save(quouteToDb);
            }
        }

        public List<QuotePoco> Get(string SymbolName)
        {
           return this.Collection.AsQueryable().
                Where(quoteData => quoteData.SymbolName == SymbolName).ToList().
                Select(e => _mapper.Map<QuotePoco>(e)).ToList();
        }
    }
}
