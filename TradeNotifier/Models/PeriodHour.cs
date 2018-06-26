using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeNotifier.Models
{
    public class PeriodHour : Period
    {
        public PeriodHour() : base(new TimeSpan(1, 0, 0)){}
    }
}
