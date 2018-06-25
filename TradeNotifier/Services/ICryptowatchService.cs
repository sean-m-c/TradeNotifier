using TradeNotifier.Models;

namespace TradeNotifier.Services
{
    public interface ICryptowatchService
    {
        CryptowatchOHLCListDTO GetOHLCs();
    }
}