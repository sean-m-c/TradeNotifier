using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeNotifier.Models
{
    public class PeriodHistoric : Period
    {
        public DateTime Start { get; }

        public PeriodHistoric(TimeSpan timeSpan, DateTime? start) : base(timeSpan)
        {
            if (start == null) throw new ArgumentNullException(nameof(start));
            if (start < DateTime.Now) throw new ArgumentOutOfRangeException(nameof(start), start, "Value cannot be greater than the current time.");

            Start = start.Value;
        }
    }
}
