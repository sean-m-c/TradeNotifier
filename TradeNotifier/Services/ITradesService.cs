using System.Collections.Generic;
using TradeNotifier.Models;
using TradeNotifier.Services.BitMEX;

namespace TradeNotifier.Services
{
    public interface ITradesService
    {
        IEnumerable<BitMEXOrderBookItemDTO> GetOrders();
    }
}