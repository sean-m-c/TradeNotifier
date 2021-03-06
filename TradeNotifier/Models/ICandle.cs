﻿using System;

namespace TradeNotifier.Models
{
    public interface ICandle
    {
        bool IsBearish { get; }
        bool IsBullish { get; }
        bool IsOpen { get; }
        DateTime CloseTimestamp { get; }
        DateTime OpenTimestamp{ get; }
        decimal Close { get; }
        decimal High { get; }
        decimal Low { get; }
        decimal Open { get; }
        IPeriod Period { get; }
    }
}