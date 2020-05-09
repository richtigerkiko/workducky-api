using System;
using System.Collections.Generic;

namespace WorkduckyLib.DataObjects.dbo.Mongo
{
    public static class MongoUtility
    {
        public static string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}