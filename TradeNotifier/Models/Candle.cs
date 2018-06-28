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
        public decimal Close => Ohlc.Close.Value;
        public decimal High => Ohlc.High.Value;
        public decimal Low => Ohlc.Low.Value;
        public decimal Open => Ohlc.Open.Value;
        public decimal PercentChange => Close != 0 ? (1 - (((Open - Close) / Open)) * 100) : 0;
        public decimal VolatilityPercent => Open != 0 ? (1 - (((High - Low) / Open)) * 100) : 0; // TODO: check this math
        public decimal VolatilityPrice => (High - Low) * -1;
        public IOhlc Ohlc { get; }
        public IPeriod Period { get; }

        public Candle(DateTime? closeTimestamp, IOhlc ohlc, IPeriod period)
        {
            if (closeTimestamp == null) throw new ArgumentNullException(nameof(closeTimestamp));
            if (period == null) throw new ArgumentNullException(nameof(period));
            if (ohlc == null) throw new ArgumentNullException(nameof(ohlc));
            ohlc.Validate();

            Ohlc = ohlc;
            CloseTimestamp = closeTimestamp.Value;
            Period = period;
        }
    }
}
