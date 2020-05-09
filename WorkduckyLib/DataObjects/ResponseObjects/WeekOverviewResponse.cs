using System;
using System.Collections.Generic;
using NodaTime;

namespace WorkduckyLib.DataObjects.ResponseObjects
{
    public class WeekOverviewResponse : AbstractResponse
    {
        public List<Timer> TimerList { get; set; }
        public Duration TotalOverTime { get; set; }
    }
}