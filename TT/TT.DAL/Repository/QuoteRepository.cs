using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
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
        private AutoMapper.IMapper _mapperQuotePoco;
        private AutoMapper.IMapper _mapperQuoteEntity;

        public QuoteRepository() : base()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<QuotePoco, QuoteEntity>());
            _mapperQuotePoco = config.CreateMapper();
            config = new MapperConfiguration(cfg => cfg.CreateMap<QuoteEntity, QuotePoco>());
            _mapperQuoteEntity = config.CreateMapper();
        }

        public void Add(IEnumerable<QuotePoco> quotes)
        {
            foreach (var quote in quotes)
            {
                var quouteToDb = _mapperQuoteEntity.Map<QuoteEntity>(quote);
                this.Collection.Save(quouteToDb);
            }
        }

        public void RemoveUntil(DateTime time)
        {
            collection.Remove(Query<QuoteEntity>.Where(entity => entity.Time < time));
        }

        public List<QuotePoco> Get(string SymbolName)
        {
           return this.Collection.AsQueryable().
                Where(quoteData => quoteData.SymbolName == SymbolName).ToList().
                Select(e => _mapperQuotePoco.Map<QuotePoco>(e)).ToList();
        }
    }
}
