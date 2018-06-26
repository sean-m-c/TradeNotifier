using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeNotifier.Models
{
    public class PeriodDay : Period
    {
        public PeriodDay() : base(new TimeSpan(1, 0, 0, 0)){}
    }
}
