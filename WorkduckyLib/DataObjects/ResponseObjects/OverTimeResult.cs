using NodaTime;

namespace WorkduckyLib.DataObjects.ResponseObjects
{
    public class OverTimeResult : AbstractResponse
    {
        public Duration OverTime { get; set; }
    }
}