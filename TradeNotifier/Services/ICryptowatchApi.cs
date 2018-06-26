using TradeNotifier.Models;

namespace TradeNotifier.Services
{
    public interface ICryptowatchApi
    {
        string GetOHLCs(IPeriod period);
    }
}