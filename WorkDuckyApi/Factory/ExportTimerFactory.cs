using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WorkDuckyAPI.Actions.FileExports;
using WorkduckyLib.DataObjects;
using WorkduckyLib.DataObjects.RequestObjects;
using WorkduckyLib.DataObjects.ResponseObjects;
using WorkduckyLib.Enums;

namespace WorkDuckyAPI.Factory
{
    public class ExportTimerFactory
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public ExportTimerFactory(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public async Task<FileExportResponse> CreateFileStream(User user, ExportTimerRequest request)
        {
            switch (request.FileFormat)
            {
                case FileFormats.CSV:
                    var fileExportResponse = new FileExportResponse();
                    fileExportResponse.MimeType = MimeTypes.Text.Csv;
                    var action = new ExportTimersToCsv(config, logger);
                    return await action.GenerateExportFile(user, request.Year);
                default:
                    throw new NotImplementedException(string.Format("Can't find Type: {0}", request.GetType().ToString()));
            }
        }
    }
}
