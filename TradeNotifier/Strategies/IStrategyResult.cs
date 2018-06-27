using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeNotifier.Models;

namespace TradeNotifier.Strategies
{
    public interface IStrategyResult
    {
        bool IsLong { get; }
        bool IsShort { get; }
        bool IsStop { get; }
        IPeriod Period { get; }
    }
}
