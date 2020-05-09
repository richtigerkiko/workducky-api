using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using WorkDuckyAPI.DataAccess.Mongo;
using WorkduckyLib.DataObjects;

namespace WorkDuckyAPI.Service
{
    public class CompanyServices
    {
        private readonly IConfiguration config;
        private readonly ILogger logger;

        public CompanyServices(IConfiguration config, ILogger logger)
        {
            this.config = config;
            this.logger = logger;
        }

        public Company GetCompany(string cid)
        {
            var db = new CompanyDataAccess(config, logger);

            var company = db.GetComanyByCompanyId(cid);

            return (Company)company;
        }
    }
}