using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeNotifier.Models
{
    public class BitMEXOrderBookItemDTO
    {
        public DateTime TimeStamp { get; set; }
        public decimal? AvgPx { get; set; }
        public decimal? OrderQty { get; set; }
        public decimal? Price { get; set; }
        public decimal? StopPx { get; set; }
        public string OrdStatus { get; set; }
        public string OrdType { get; set; }
        public string Side { get; set; }
        public string Text { get; set; }
        public string OrderId { get; set; }
        public string Symbol { get; set; }

    }

}
