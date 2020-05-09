using System;
using System.Collections.Generic;
using WorkduckyLib.Enums;

namespace WorkduckyLib.DataObjects.dbo.Mongo
{
    public class AuthenticationDocument
    {
        public AuthenticationTypes AuthType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime DateAdded { get; set; }
        public List<AuthenticationAttempt> AuthenticationLog { get; set; }
        public bool isBlocked { get; set; }
        public bool isActive { get; set; }
        public int FailedAttempts { get; set; }
        public string _id { get; set; }

        public AuthenticationDocument()
        {
            DateAdded = DateTime.UtcNow;
            _id = MongoUtility.GenerateId();
        }

    }

    public class AuthenticationAttempt
    {
        public DateTime DateAdded { get; set; }
        public bool Successfull { get; set; }
    }
}