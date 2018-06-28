namespace TradeNotifier.Models
{
    public interface IOhlc
    {
        decimal? Close { get; set; }
        decimal? High { get; set; }
        decimal? Low { get; set; }
        decimal? Open { get; set; }

        bool IsValid();
        void Validate();
    }
}