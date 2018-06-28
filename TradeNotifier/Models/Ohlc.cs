using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeNotifier.Models
{
    public class Ohlc : IOhlc
    {
        public decimal? Open { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Close { get; set; }

        public void Validate()
        {
            if (High == null) throw new ArgumentNullException(nameof(High));
            if (Low == null) throw new ArgumentNullException(nameof(Low));
            if (Open == null) throw new ArgumentNullException(nameof(Open));
            if (High < Low) throw new ArgumentOutOfRangeException(nameof(High), High, $"Value of High cannot be less than value of Low. Value of Low: ${Low}.");
            if (High < Close) throw new ArgumentOutOfRangeException(nameof(High), High, $"Value of High cannot be less than value of close. Value of close: ${Close}.");
            if (High < Open) throw new ArgumentOutOfRangeException(nameof(High), High, $"Value of High cannot be less than value of Open. Value of Open: ${Open}.");
            if (Low > Close) throw new ArgumentOutOfRangeException(nameof(Low), Low, $"Value of Low cannot be greater than value of close. Value of close: ${Close}.");
            if (Low > Open) throw new ArgumentOutOfRangeException(nameof(Low), Low, $"Value of Low cannot be greater than value of Open. Value of Open: ${Open}.");
        }

        public bool IsValid()
        {
            try
            {
                Validate();
                return true;
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                return false;
            }
        }
    }
}
