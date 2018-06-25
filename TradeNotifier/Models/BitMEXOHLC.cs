using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeNotifier.Models
{
    public class BitMEXOHLCDTO
    {
        public DateTime Timestamp { get; set; } 
        public int AskPrice { get; set; }
        public int AskSize { get; set; }
        public int BidPrice { get; set; }
        public int BidSize { get; set; } 
        public string Symbol { get; set; } 
    }
}
