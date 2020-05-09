using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using WorkduckyLib.DataObjects.Configuration;

namespace WorkDuckyAPI.DataAccess.Mongo
{
    public class MongoDBConn
    {
        private MongoClient client;
        public IMongoDatabase db;

        public MongoDBConfiguration mongoDBConfiguration;


        public MongoDBConn(IConfiguration configuration, ILogger logger)
        {
            try
            {
                mongoDBConfiguration = new MongoDBConfiguration();
                configuration.GetSection("mongodb").Bind(mongoDBConfiguration);
                client = new MongoClient(mongoDBConfiguration.ConnectionString);
                db = client.GetDatabase(mongoDBConfiguration.DatabaseName);
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Oh Shit");
                throw ex;
            }
        }
    }
}