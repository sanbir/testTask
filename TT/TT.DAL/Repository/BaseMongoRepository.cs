using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoRepository;
using TT.Core.Settings;

namespace TT.DAL.Repository
{
    public abstract class BaseMongoRepository<TEntity> : MongoRepository<TEntity> where TEntity : MongoRepository.Entity
    {
        public BaseMongoRepository() : base(TTSettings.MongoConnectionString)
        {
            
        }
    }
}
