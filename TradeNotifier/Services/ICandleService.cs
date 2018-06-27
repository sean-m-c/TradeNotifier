using System.Collections.Generic;
using System.Threading.Tasks;
using TradeNotifier.Models;

namespace TradeNotifier.Services
{
    public interface ICandleService
    {
        Task<List<ICandle>> GetCandlesAsync(IPeriod period);
    }
}