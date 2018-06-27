using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeNotifier.Models;

namespace TradeNotifier.Strategies
{
    public interface IStrategy
    {
        IStrategyResult GetStrategyResult(IPeriod period);
    }
}
