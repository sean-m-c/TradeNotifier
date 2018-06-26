using System;

namespace TradeNotifier.Models
{
    public class Period : IPeriod
    {
        public TimeSpan PeriodTimeSpan { get; }

        public Period(TimeSpan timeSpan)
        {
            if (timeSpan == null) throw new ArgumentNullException(nameof(timeSpan));
            if (timeSpan.CompareTo(TimeSpan.Zero) < 0) throw new ArgumentOutOfRangeException(nameof(timeSpan), timeSpan, "Value must be greater than zero.");

            PeriodTimeSpan = timeSpan;
        }

        public string ToDisplayFormat()
        {
            if (PeriodTimeSpan.Days >= 7)
            {
                return $"{(PeriodTimeSpan.Days > 7 ? (PeriodTimeSpan.Days / 7).ToString() : string.Empty)}W";
            }
            else if (PeriodTimeSpan.Days > 1)
            {
                return $"{(PeriodTimeSpan.Days > 1 ? PeriodTimeSpan.Days.ToString() : string.Empty)}D";
            }
            else if (PeriodTimeSpan.Hours >= 1)
            {
                return $"{PeriodTimeSpan.Hours}h";
            }
            else if (PeriodTimeSpan.Minutes >= 1)
            {
                return $"{PeriodTimeSpan.Minutes}m";
            }
            else
            {
                return $"{PeriodTimeSpan.Seconds}";
            }
        }
    }
}
