using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.DAL.Pocos
{
    public class QuotePoco
    {
        public string Symbol { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public decimal ChangePoints { get; set; }
        public decimal ChangePercent { get; set; }
        public DateTime Time { get; set; }
    }
}
