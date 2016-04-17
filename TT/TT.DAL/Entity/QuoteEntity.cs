using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MongoRepository;

namespace TT.DAL.Entity
{
    [CollectionName("CURRENCY_DATA")]
    public class QuoteEntity : MongoRepository.Entity
    {
        //"S": "EURJPY", - symbol name
        //"B": 126.831, - bid value
        public string Symbol { get; set; }
        public decimal Bid { get; set; }
        public DateTime Time { get; set; }
    }
}
