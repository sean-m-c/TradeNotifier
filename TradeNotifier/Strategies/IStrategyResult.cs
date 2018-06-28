using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeNotifier.Models;

namespace TradeNotifier.Strategies
{
    public interface IStrategyResult
    {
        IEnumerable<ICandle> Candles { get; }
        bool IsLong { get; }
        bool IsLongStop { get; }
        bool IsShort { get; }
        bool IsShortStop { get; }
        IPeriod Period { get; }
    }
}
