namespace TradeNotifier.Services.BitMEX
{
    public interface IBitMEXApi
    {
        string DeleteOrders();
        string GetOrders();
        string PostOrders();
    }
}