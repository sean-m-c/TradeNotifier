using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeNotifier.Models
{
    public class PeriodFourHour : Period
    {
        public PeriodFourHour() : base(new TimeSpan(4, 0, 0)){}
    }
}
