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

        public int RoundedMinutes => (int)PeriodTimeSpan.TotalMinutes;

        public int RoundedSeconds => (int)PeriodTimeSpan.TotalSeconds;

        public string HumanizedName
        {
            get
            {
                if (_humanizedName == null)
                {
                    if (PeriodTimeSpan.Days >= 7)
                    {
                        _humanizedName = $"{(PeriodTimeSpan.Days > 7 ? (PeriodTimeSpan.Days / 7).ToString() : string.Empty)}W";
                    }
                    else if (PeriodTimeSpan.Days > 1)
                    {
                        _humanizedName = $"{(PeriodTimeSpan.Days > 1 ? PeriodTimeSpan.Days.ToString() : string.Empty)}D";
                    }
                    else if (PeriodTimeSpan.Hours >= 1)
                    {
                        _humanizedName = $"{PeriodTimeSpan.Hours}h";
                    }
                    else if (PeriodTimeSpan.Minutes >= 1)
                    {
                        _humanizedName = $"{PeriodTimeSpan.Minutes}m";
                    }
                    else
                    {
                        _humanizedName = $"{PeriodTimeSpan.Seconds}";
                    }
                }

                return _humanizedName;
            }
        }
        private string _humanizedName;


    public DateTime NextClose() => DoCalculateNextCloseTime(DateTime.Now, this.PeriodTimeSpan);

    public DateTime NextClose(DateTime? startTime)
    {
        return DoCalculateNextCloseTime(startTime, this.PeriodTimeSpan);
    }

    DateTime DoCalculateNextCloseTime(DateTime? startTime, TimeSpan? timeSpan)
    {
        if (startTime == null) throw new ArgumentNullException(nameof(startTime));
        if (timeSpan == null) throw new ArgumentNullException(nameof(timeSpan));

        // Round up to next timespan interval.
        return new DateTime((startTime.Value.Ticks + timeSpan.Value.Ticks - 1) / timeSpan.Value.Ticks * timeSpan.Value.Ticks, startTime.Value.Kind);
    }
}
}
