using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.DAL.Entity
{
    public class QuoteEntity : MongoRepository.Entity
    {
        //"S": "EURJPY", - symbol name
        //"B": 126.831, - bid value
        //"A": 126.852, - ask value
        //"CHG": 35.8, - change in points
        //"CHG%": 0.28, - change in %
        public string SymbolName { get; set; }
        public decimal BidValue { get; set; }
        public decimal AskValue { get; set; }
        public decimal ChangePoints { get; set; }
        public decimal GhangePercent { get; set; }
    }
}
