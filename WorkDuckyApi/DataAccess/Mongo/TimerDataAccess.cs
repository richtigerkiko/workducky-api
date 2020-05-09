using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NodaTime.Calendars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkduckyLib.DataObjects;
using WorkduckyLib.DataObjects.dbo.Mongo;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.DataObjects.ResponseObjects;
using WorkduckyLib.Interfaces;

namespace WorkDuckyAPI.DataAccess.Mongo
{
    public class TimerDataAccess : MongoDBConn
    {
        private IMongoCollection<TimerDocument> timerCollection = null;

        private readonly ILogger logger;

        public TimerDataAccess(IConfiguration configuration, ILogger logger) : base(configuration, logger)
        {
            if (configuration != null) timerCollection = db.GetCollection<TimerDocument>("timers");
            if (logger != null) this.logger = logger;
        }

        public TimerResponse WriteStartTimerToDb(Timer timer, IUser user)
        {
            // Check if no timer is in started state
            if (timerCollection.Find(x => x.UserRef == user.Uid).ToList().Any(d => d.TimerState == TimerState.Started))
            {
                throw new Exception("Timer already started");
            }
            TimerDocument timerDocument = new TimerDocument(timer);
            timerDocument.UserRef = user.Uid;
            timerCollection.InsertOne(timerDocument);
            var response = new TimerResponse();
            response.TimerId = timerDocument.TimerId;
            return response;
        }

        public Timer GetTimer(string timerId)
        {
            Timer timer = timerCollection.Find(x => x.TimerId == timerId).FirstOrDefault();
            return timer;
        }

        public TimerResponse StopTimer(Timer timer)
        {
            var filter = Builders<TimerDocument>.Filter.Eq("TimerId", timer.TimerId);
            var update = Builders<TimerDocument>.Update.Set("TimerState", timer.TimerState)
                                                       .Set("EndTimer", timer.EndTimer)
                                                       .Set("TotalDurationWorked", timer.TotalDurationWorked);
            timerCollection.UpdateOne(filter, update);
            return new TimerResponse()
            {
                TimerId = timer.TimerId,
                Timer = timer
            };
        }

        public void RateTimer(TimerReflection request, string timerId)
        {
            var filter = Builders<TimerDocument>.Filter.Eq("TimerId", timerId);
            var update = Builders<TimerDocument>.Update.Set("TimerReflection", request);

            timerCollection.UpdateOne(filter, update);
        }

        public void StartBreak(BreakObject breakObject, string timerId)
        {
            var timerDoc = timerCollection.Find(x => x.TimerId == timerId).FirstOrDefault();
            if (timerDoc.WorkBreak == null)
            {
                timerDoc.WorkBreak = new WorkBreak();
            }
            timerDoc.WorkBreak.BreakList.Add(breakObject);

            var filter = Builders<TimerDocument>.Filter.Eq("TimerId", timerId);
            var update = Builders<TimerDocument>.Update.Set("WorkBreak", timerDoc.WorkBreak);
            timerCollection.UpdateOne(filter, update);
        }

        public WorkBreak GetCurrentWorkBreak(BreakRequest request)
        {
            var workBreak = timerCollection.Find(x => x.TimerId == request.TimerId)
                                            .FirstOrDefault()
                                            .WorkBreak;
            return workBreak;
        }

        public Timer GetCurrentTimer(User user)
        {
            return timerCollection.Find(x => x.TimerState == TimerState.Started).FirstOrDefault();

        }

        public void SetCurrentWorkBreak(string timerId, WorkBreak workBreak)
        {
            var filter = Builders<TimerDocument>.Filter.Eq("TimerId", timerId);
            var update = Builders<TimerDocument>.Update.Set("WorkBreak", workBreak);
            timerCollection.UpdateOne(filter, update);
        }

        public void AddDummyTimers(List<Timer> timerList, string userId)
        {
            try
            {
                var timerDocList = new List<TimerDocument>();
                foreach (var timer in timerList)
                {
                    var timerDoc = new TimerDocument(timer);
                    timerDoc.UserRef = userId;
                    timerDocList.Add(timerDoc);
                }
                timerCollection.InsertMany(timerDocList);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task<List<Timer>> GetTimersAsync(string uid, int year, int week)
        {
            var rule = WeekYearRules.Iso;
            var timerList = new List<Timer>();
            var findings = await timerCollection.FindAsync(x => x.UserRef == uid && x.Year == year && x.WeekNumber == week);
            findings.ToList().ForEach(x => timerList.Add(x));
            return timerList;
        }

        public async Task<List<Timer>> GetAllUserTimersOfYear(string uid, int year)
        {
            var timerList = new List<Timer>();
            var collection = await timerCollection.FindAsync(x => x.Year == year && x.UserRef == uid);
            collection.ToList().ForEach(x => timerList.Add(x));
            return timerList;
        }
    }
}