using System;
using System.Collections.Generic;
using NodaTime;

namespace WorkduckyLib.DataObjects
{
    public class Timer
    {
        public string TimerId { get; set; }
        public TimerType TimerType { get; set; }
        public TimerEntry StartTimer { get; set; }
        public TimerEntry EndTimer { get; set; }
        public TimerState TimerState { get; set; }
        public TimerReflection TimerReflection { get; set; }
        public WorkBreak WorkBreak { get; set; }
        public Duration TotalDurationWorked { get; set; }

        public Timer()
        {
            TimerId = Guid.NewGuid().ToString("N");
        }
    }

    public enum TimerState
    {
        Stopped = 0,
        Started = 1
    }

    public enum TimerType
    {
        NormalDay = 0,
        PublicHoliday = 1,
        Holiday = 2,
        Sickleave = 3
    }
    public class TimerEntry
    {
        public ZonedDateTime Time { get; set; }
        public Location Location { get; set; }

        public TimerEntry()
        {
            Time = ZonedDateTime.FromDateTimeOffset(DateTimeOffset.Now);
        }
    }

    public class TimerReflection
    {
        public int Score { get; set; }
        public string Notes { get; set; }
    }

    public class WorkBreak
    {
        public List<BreakObject> BreakList { get; set; }
        public Duration TotalBreakDuration { get; set; }

        public WorkBreak()
        {
            BreakList = new List<BreakObject>();
        }
    }

    public class BreakObject
    {
        public string BreakId { get; set; }
        public ZonedDateTime Start { get; set; }
        public ZonedDateTime End { get; set; }
        public Duration BreakDuration { get; set; }
        public TimerState State { get; set; }

        public BreakObject()
        {
            BreakId = Guid.NewGuid().ToString("N");
        }
    }
}