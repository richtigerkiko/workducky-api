using System;
using System.Collections.Generic;

namespace WorkduckyLib.DataObjects.dbo.Mongo
{
    public class UserDocument
    {
        public string _id { get; set; }
        public List<AuthenticationDocument> AuthenticationDocuments { get; set; }
        public string Uid { get; set; }
        public string CompanyReference { get; set; }
        public Settings UserSettings { get; set; }

        public UserDocument()
        {
            _id = MongoUtility.GenerateId();
            Uid = _id;
        }
    }
}