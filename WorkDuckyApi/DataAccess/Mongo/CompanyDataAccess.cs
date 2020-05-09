using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using WorkduckyLib.DataObjects.dbo.Mongo;
using WorkduckyLib.Interfaces;

namespace WorkDuckyAPI.DataAccess.Mongo
{
    public class CompanyDataAccess : MongoDBConn
    {

        private IMongoCollection<CompanyDocument> companyCollection = null;

        public CompanyDataAccess(IConfiguration configuration, ILogger logger): base(configuration, logger)
        {
            companyCollection = db.GetCollection<CompanyDocument>("companies");
        }

        public CompanyDocument GetComanyByName(string companyName)
        {
            throw new NotImplementedException();
        }

        public CompanyDocument GetComanyByCompanyId(string cid)
        {
            return companyCollection.Find(x => x._id == cid).FirstOrDefault();
        }
    }

}