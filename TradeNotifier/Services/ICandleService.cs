using TradeNotifier.Models;

namespace TradeNotifier.Services
{
    public interface ICandleService
    {
        decimal CalculateShortCBL(CryptowatchOHLCDTO[] candles);
    }
}