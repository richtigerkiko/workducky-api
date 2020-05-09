using System;

namespace WorkduckyLib.DataObjects
{
    public class TokenPayload
    {
        public long ExpirationDate { get; set; }
        public string Uid { get; set; }
    }
}