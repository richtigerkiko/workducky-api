using System;

namespace WorkduckyLib.DataObjects.ResponseObjects
{
    public class TimerResponse : AbstractResponse
    {
        public string TimerId { get; set; }
        public Timer Timer { get; set; }
    }
}