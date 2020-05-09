using System.Collections.Generic;

namespace WorkduckyLib.DataObjects
{
    public class Settings
    {
        public List<Location> Locations { get; set; }
        public WorkdayDefaults WorkdayDefaults { get; set; }
    }
}