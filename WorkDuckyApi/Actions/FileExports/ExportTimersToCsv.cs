using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WorkDuckyAPI.DataAccess.Mongo;
using WorkduckyLib.DataObjects;
using WorkduckyLib.DataObjects.ResponseObjects;

namespace WorkDuckyAPI.Actions.FileExports
{
    public class ExportTimersToCsv
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public ExportTimersToCsv(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public async Task<FileExportResponse> GenerateExportFile(User user, int year)
        {
            var db = new TimerDataAccess(config, logger);
            var timers = await db.GetAllUserTimersOfYear(user.Uid, year);
            var csv = await GenerateCSVFromTimerList(timers);
            var fileExportResponse = new FileExportResponse();
            fileExportResponse.FileStream = StringToStream(csv);
            fileExportResponse.MimeType = MimeTypes.Text.Csv;
            return fileExportResponse;
        }

        private async Task<string> GenerateCSVFromTimerList(List<Timer> timers)
        {
            return await Task.Run(() =>
            {
                var csv = "TimerId;TimerType;Date;FromTime;ToTime;TotalWorkDuration;TotalBreakDuration;Rating;Notes;" + Environment.NewLine;
                foreach (var timer in timers)
                {
                    csv += string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};", 
                        timer.TimerId, 
                        timer.TimerType.ToString(),
                        timer.StartTimer.Time.Date.ToString("d", null), 
                        timer.StartTimer.Time.TimeOfDay.ToString(), 
                        timer.EndTimer.Time.TimeOfDay.ToString(),
                        timer.TotalDurationWorked.ToString(),
                        timer.WorkBreak.TotalBreakDuration.ToString(),
                        timer.TimerReflection.Score,
                        timer.TimerReflection.Notes,
                        Environment.NewLine);
                }
                return csv;
            });
        }

        private Stream StringToStream(string stringToConvert)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(stringToConvert);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
