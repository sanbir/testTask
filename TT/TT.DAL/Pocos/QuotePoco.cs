using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.DAL.Pocos
{
    public class QuotePoco
    {
        public string SymbolName { get; set; }
        public decimal BidValue { get; set; }
        public decimal AskValue { get; set; }
        public decimal ChangePoints { get; set; }
        public decimal GhangePercent { get; set; }
        public DateTime Time { get; set; }
    }
}
