using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using TT.DAL.Entity;
using TT.DAL.Pocos;
using static AutoMapper.Mapper;

namespace TT.DAL.Repository
{
    public class QuoteRepository : BaseMongoRepository<QuoteEntity>, IQuoteRepository
    {
        public QuoteRepository() : base()
        {
            CreateMap<QuotePoco, QuoteEntity>();
            CreateMap<QuoteEntity, QuotePoco>();
        }

        public void Add(IEnumerable<QuotePoco> quotes)
        {
            this.Collection.InsertBatch(quotes);
        }

        public void RemoveUntil(DateTime time)
        {
            collection.Remove(Query<QuoteEntity>.Where(entity => entity.Time < time));
        }

        public List<QuotePoco> Get(string symbol)
        {
            List<QuoteEntity> quoteEntities =
                this.Collection.AsQueryable().Where(quoteData => quoteData.Symbol == symbol).ToList();
            List<QuotePoco> quotePocos = quoteEntities.Select(Map<QuotePoco>).ToList();

            return quotePocos;
        }

        public List<QuotePoco> Get(string symbol, DateTime timeToGetAfter)
        {
            List<QuoteEntity> quoteEntities =
                this.Collection.AsQueryable().Where(quoteData => quoteData.Symbol == symbol && quoteData.Time > timeToGetAfter).ToList();
            List<QuotePoco> quotePocos = quoteEntities.Select(Map<QuotePoco>).ToList();

            return quotePocos;
        }
    }
}