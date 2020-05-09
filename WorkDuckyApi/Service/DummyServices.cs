using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NodaTime;
using NodaTime.Text;
using WorkDuckyAPI.DataAccess.Mongo;
using WorkduckyLib.DataObjects;

namespace WorkDuckyAPI.Service
{
    public class DummyServices
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public DummyServices(IConfiguration config, ILogger logger)
        {
            this.logger = logger;
            this.config = config;
        }
        private int GetRandomInt()
        {
            var random = new Random();
            return random.Next(-100, 100);
        }
        private int GetRandomInt(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }

        private ZonedDateTime GenerateRandomTime(ZonedDateTime time, int hours)
        {
            var timeString = time.ToString("G", CultureInfo.InvariantCulture);
            var replacementString = String.Format("{0}:{1}:{2}", hours.ToString("00"), (40 + GetRandomInt(-10, 10)).ToString(), (40 + GetRandomInt(-10, 10)).ToString());
            var newTimeString = Regex.Replace(timeString, @"\d*:\d*:\d*", replacementString);
            newTimeString = Regex.Replace(newTimeString, @" UTC.*", "");
            var parser = ZonedDateTimePattern.CreateWithInvariantCulture("yyyy'-'M'-'d'T'HH:mm:ss", DateTimeZoneProviders.Serialization);
            return parser.Parse(newTimeString).GetValueOrThrow();
        }
                // Generates one year of dummy Timer Data
        public void GenerateDummyData(string userId)
        {
            var timerList = new List<Timer>();
            var now = ZonedDateTime.FromDateTimeOffset(DateTime.Now);
            for (int i = 365; i > 0; i--)
            {
                var then = ZonedDateTime.Subtract(now, Duration.FromDays(i));
                if (then.DayOfWeek != IsoDayOfWeek.Saturday && then.DayOfWeek != IsoDayOfWeek.Sunday)
                {
                    var startTime = GenerateRandomTime(then, 8);
                    var endTime = GenerateRandomTime(then, 17);
                    var breakStart = GenerateRandomTime(then, 13);
                    var breakEnd = breakStart.PlusMinutes(GetRandomInt(25, 35));
                    var workBreak = new WorkBreak()
                    {
                        BreakList = new List<BreakObject>(){
                            new BreakObject(){
                                Start = breakStart,
                                End = breakEnd,
                                BreakDuration = (breakEnd - breakStart),
                                State = TimerState.Stopped
                            }
                        },
                        TotalBreakDuration = (breakEnd - breakStart)
                    };
                    var timer = new Timer()
                    {
                        StartTimer = new TimerEntry()
                        {
                            Location = new Location()
                            {
                                Coordinates = new Coordinate()
                                {
                                    Latitude = 50.941864013671875F - (GetRandomInt(0, 100) / 100000),
                                    Longitude = 6.95757532119751F - (GetRandomInt(0, 100) / 100000)
                                },
                                LocationName = "dummylocation",
                                RadiusInMeters = GetRandomInt(0, 35)
                            },
                            Time = startTime
                        },
                        EndTimer = new TimerEntry()
                        {
                            Location = new Location()
                            {
                                Coordinates = new Coordinate()
                                {
                                    Latitude = 50.941864013671875F - (GetRandomInt() / 100000),
                                    Longitude = 6.95757532119751F - (GetRandomInt() / 100000)
                                },
                                LocationName = "dummylocation",
                                RadiusInMeters = GetRandomInt(0, 35)
                            },
                            Time = endTime
                        },
                        TimerReflection = new TimerReflection()
                        {
                            Notes = "dummydummy",
                            Score = GetRandomInt(2, 10)
                        },
                        TimerState = TimerState.Stopped,
                        TimerType = (TimerType)GetRandomInt(0, 3),
                        TotalDurationWorked = (endTime - startTime),
                        WorkBreak = workBreak
                    };
                    timerList.Add(timer);
                }
            }
            var db = new TimerDataAccess(config, logger);
            db.AddDummyTimers(timerList, userId);
        }
    }
}