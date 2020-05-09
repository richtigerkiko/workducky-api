using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NodaTime.Calendars;

namespace WorkduckyLib.DataObjects.dbo.Mongo
{
    public class TimerDocument: Timer
    {
        public string _id { get; set; }
        public string UserRef { get; set; }
        public string MachineId { get; set; }
        public int Year { get; set; }
        public int WeekNumber { get; set; }

        public TimerDocument()
        {
            _id = TimerId;
            WorkBreak = new WorkBreak();
            WorkBreak.BreakList = new List<BreakObject>();
        }

        public TimerDocument(Timer timer){
            TimerId = timer.TimerId;
            _id = TimerId;
            WorkBreak = timer.WorkBreak;
            EndTimer = timer.EndTimer;
            StartTimer = timer.StartTimer;
            TimerState = timer.TimerState;
            TimerType = timer.TimerType;
            TimerReflection = timer.TimerReflection;
            TotalDurationWorked = timer.TotalDurationWorked;
            
            var rule = WeekYearRules.Iso;
            WeekNumber = rule.GetWeekOfWeekYear(timer.StartTimer.Time.Date);
            Year = timer.StartTimer.Time.Year;

        }
    }
}