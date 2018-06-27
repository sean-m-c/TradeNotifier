using System;

namespace TradeNotifier.Models
{
    public interface IPeriod
    {
        DateTime NextClose();
        DateTime NextClose(DateTime? startTime);
        int RoundedMinutes { get; }
        int RoundedSeconds { get; }
        string HumanizedName { get; }
        TimeSpan PeriodTimeSpan { get; }
    }
}