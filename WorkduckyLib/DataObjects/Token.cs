using System;

namespace WorkduckyLib.DataObjects
{
    public class Token
    {
        public string jwt { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }
    }
}