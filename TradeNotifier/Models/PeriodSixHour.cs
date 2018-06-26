using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeNotifier.Models
{
    public class PeriodSixHour : Period
    {
        public PeriodSixHour() : base(new TimeSpan(6, 0, 0)){}
    }
}
