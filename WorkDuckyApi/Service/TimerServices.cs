using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkDuckyAPI.DataAccess.Mongo;
using WorkduckyLib.DataObjects;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.DataObjects.ResponseObjects;
using WorkduckyLib.Interfaces;
using NodaTime;
using System.Linq;
using NodaTime.Calendars;

namespace WorkDuckyAPI.Service
{
    public class TimerServices
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public TimerServices(IConfiguration config, ILogger logger)
        {
            this.logger = logger;
            this.config = config;
        }

        public TimerResponse StartTimer(StartTimerRequest request, IUser user)
        {
            var db = new TimerDataAccess(config, logger);

            var timer = new Timer()
            {
                StartTimer = new TimerEntry()
                {
                    Location = request.Location
                },
                TimerType = request.Type
            };

            if (request.Type == TimerType.NormalDay)
            {
                timer.TimerState = TimerState.Started;
            }

            var response = new TimerResponse();

            response = db.WriteStartTimerToDb(timer, user);

            return response;
        }

        public TimerResponse StopTimer(StopTimerRequest request)
        {
            var db = new TimerDataAccess(config, logger);
            var timer = db.GetTimer(request.TimerId);
            if (timer == null)
            {
                throw new Exception("Timer not found");
            }
            timer.EndTimer = new TimerEntry()
            {
                Location = request.Location
            };
            timer.TimerState = TimerState.Stopped;

            timer.TotalDurationWorked = (timer.EndTimer.Time - timer.StartTimer.Time);

            var response = db.StopTimer(timer);

            return response;
        }

        public void RateTimer(RateTimerRequest request)
        {
            var db = new TimerDataAccess(config, logger);
            var timerReflection = new TimerReflection()
            {
                Notes = request.Notes,
                Score = request.Score
            };
            db.RateTimer(timerReflection, request.TimerId);
        }

        public void StartBreak(BreakRequest request)
        {
            var db = new TimerDataAccess(config, logger);
            var breakObject = new BreakObject()
            {
                Start = ZonedDateTime.FromDateTimeOffset(DateTime.Now),
                State = TimerState.Started
            };

            db.StartBreak(breakObject, request.TimerId);
        }

        public TimerResponse GetActiveTimer(User user)
        {
            var db = new TimerDataAccess(config, logger);
            var timerResponse = new TimerResponse();
            timerResponse.Timer = db.GetCurrentTimer(user);
            return timerResponse;
        }

        public void StopBreak(BreakRequest request)
        {
            var db = new TimerDataAccess(config, logger);
            var workBreak = db.GetCurrentWorkBreak(request);

            var breakObject = workBreak.BreakList.Where(x => x.State == TimerState.Started)
                                                 .FirstOrDefault();

            if (breakObject == null)
            {
                throw new Exception("No Active Break Found");
            }

            breakObject.End = ZonedDateTime.FromDateTimeOffset(DateTime.Now);
            breakObject.BreakDuration = (breakObject.End - breakObject.Start);
            breakObject.State = TimerState.Stopped;

            workBreak.TotalBreakDuration = workBreak.TotalBreakDuration + breakObject.BreakDuration;

            db.SetCurrentWorkBreak(request.TimerId, workBreak);
        }

        public WeekOverviewResponse GetWeekOverview(int year, int week)
        {
            var now = LocalDate.FromDateTime(DateTime.Now);
            if (year == 0)
            {
                year = now.Year;
            }
            if (week == 0)
            {
                var rule = WeekYearRules.Iso;
                week = rule.GetWeekOfWeekYear(now);
            }

            var db = new TimerDataAccess(config, logger);

            var weekOverviewResponse = new WeekOverviewResponse();
            weekOverviewResponse.TimerList = db.GetTimers(year, week);
            return weekOverviewResponse;
        }
        public OverTimeResult CalculateOvertime(User user)
        {
            var db = new TimerDataAccess(config, logger);
            var timers = db.GetAllUserTimersOfYear(user.Uid, DateTime.UtcNow.Year);
            var timerDic = SortTimersByWeek(timers);
            var timeWorkedPerWeek = new Dictionary<int, Duration>();
            var currentWeek = WeekYearRules.Iso.GetWeekOfWeekYear(LocalDate.FromDateTime(DateTime.Now));
            var overTime = Duration.Zero;

            foreach (var week in timerDic)
            {
                var dayDictionary = SortTimersByDay(week.Value);

                foreach (var day in dayDictionary)
                {
                    if (day.Key == 0 || day.Key == 6)
                    {
                        day.Value.ForEach(x =>
                        {
                            overTime += x.TotalDurationWorked;
                        });
                    }
                    else
                    {
                        var dayWorkTime = Duration.Zero;
                        day.Value.ForEach(x =>
                        {
                            dayWorkTime += x.TotalDurationWorked;
                        });
                        overTime += dayWorkTime.Minus(Duration.FromHours(8));
                    }

                }
            }

            var response = new OverTimeResult()
            {
                OverTime = overTime
            };
            return response;
        }

        public Dictionary<int, List<Timer>> SortTimersByWeek(List<Timer> timers)
        {
            var timerDic = new Dictionary<int, List<Timer>>();

            timers.ForEach(x =>
            {
                var weekNumber = WeekYearRules.Iso.GetWeekOfWeekYear(x.StartTimer.Time.Date);
                if (timerDic.ContainsKey(weekNumber))
                {
                    timerDic[weekNumber].Add(x);
                }
                else
                {
                    timerDic.Add(weekNumber, new List<Timer>() { x });
                }
            });

            return timerDic;
        }
        public Dictionary<int, List<Timer>> SortTimersByDay(List<Timer> timers)
        {
            var timerDic = new Dictionary<int, List<Timer>>();

            timers.ForEach(x =>
            {
                var dayNumber = (int)x.StartTimer.Time.DayOfWeek;
                if (timerDic.ContainsKey(dayNumber))
                {
                    timerDic[dayNumber].Add(x);
                }
                else
                {
                    timerDic.Add(dayNumber, new List<Timer>() { x });
                }
            });

            return timerDic;
        }

        public TimerResponse GetTimer(string id)
        {
            var db = new TimerDataAccess(config, logger);
            var timerResponse = new TimerResponse();
            timerResponse.Timer = db.GetTimer(id);
            return timerResponse;
        }
    }
}