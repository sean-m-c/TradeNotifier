using System;

namespace TradeNotifier.Models
{
    public interface IPeriod
    {
        TimeSpan PeriodTimeSpan { get; }

        string ToDisplayFormat();
    }
}