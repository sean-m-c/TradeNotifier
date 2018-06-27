using System.Collections.Generic;
using System.Threading.Tasks;
using TradeNotifier.Models;

namespace TradeNotifier.Services
{
    public interface ICandleService
    {
        decimal CalculateShortCBL(ICandle[] candles);
        Task<List<ICandle>> GetCandlesAsync(IPeriod period);
    }
}