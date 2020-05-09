using NodaTime;
namespace WorkduckyLib.DataObjects
{
    public class WorkdayDefaults
    {
        public int breakMinutes { get; set; }
        public LocalTime defaultStartTime { get; set; }
        public LocalTime defaultEndTime { get; set; }
        public DefaultWorkDays DefaultWorkDays { get; set; }
    }
}