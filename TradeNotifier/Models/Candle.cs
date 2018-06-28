using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeNotifier.Models
{
    public class Candle : ICandle
    {
        public bool IsBearish => Open > Close;
        public bool IsBullish => Close > Open;
        public bool IsFlat => Open == Close;
        public bool IsOpen => CloseTimestamp > DateTime.Now;
        public DateTime CloseTimestamp { get; }
        public DateTime OpenTimestamp => CloseTimestamp.Subtract(this.Period.PeriodTimeSpan);
        public decimal Close { get; }
        public decimal High { get; }
        public decimal Low { get; }
        public decimal Open { get; }
        public IPeriod Period { get; }
        public decimal PercentChange => Close != 0 ? (1 - (((Open - Close) / Open)) * 100) : 0;
        public decimal VolatilityPercent => Open != 0 ? (1 - (((High - Low) / Open)) * 100) : 0; // TODO: check this math
        public decimal VolatilityPrice => (High - Low) * -1;

        public Candle(DateTime? closeTimestamp, decimal? high, decimal? low, decimal? open, decimal? close, IPeriod period)
        {
            if (closeTimestamp == null) throw new ArgumentNullException(nameof(closeTimestamp));
            if (close == null) throw new ArgumentNullException(nameof(close));
            if (high == null) throw new ArgumentNullException(nameof(high));
            if (low == null) throw new ArgumentNullException(nameof(low));
            if (open == null) throw new ArgumentNullException(nameof(open));
            if (period == null) throw new ArgumentNullException(nameof(period));

            if (high < low) throw new ArgumentOutOfRangeException(nameof(high), high, $"Value of high cannot be less than value of low. Value of low: ${low}.");

            if (high < close) throw new ArgumentOutOfRangeException(nameof(high), high, $"Value of high cannot be less than value of close. Value of close: ${close}.");
            if (high < open) throw new ArgumentOutOfRangeException(nameof(high), high, $"Value of high cannot be less than value of open. Value of open: ${open}.");


            if (low > close) throw new ArgumentOutOfRangeException(nameof(low), low, $"Value of low cannot be greater than value of close. Value of close: ${close}.");
            if (low > open) throw new ArgumentOutOfRangeException(nameof(low), low, $"Value of low cannot be greater than value of open. Value of open: ${open}.");

            Close = close.Value;
            CloseTimestamp = closeTimestamp.Value;
            High = high.Value;
            Low = low.Value;
            Open = open.Value;
            Period = period;
        }
    }
}
