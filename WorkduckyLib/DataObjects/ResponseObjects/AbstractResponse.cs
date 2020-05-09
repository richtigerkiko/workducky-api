using System;

namespace WorkduckyLib.DataObjects.ResponseObjects
{
    public class AbstractResponse
    {
        public DateTime timestamp { get; }

        public AbstractResponse()
        {
            timestamp = DateTime.UtcNow;
        }
    }
}