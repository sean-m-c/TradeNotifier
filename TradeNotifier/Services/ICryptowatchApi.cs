using System.Threading.Tasks;
using TradeNotifier.Models;

namespace TradeNotifier.Services
{
    public interface ICryptowatchApi
    {
        Task<string> GetPeriodOHLCsAsync(IPeriod period);
    }
}