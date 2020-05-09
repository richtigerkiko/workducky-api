using System.ComponentModel.DataAnnotations;
using WorkduckyLib.Interfaces;

namespace WorkduckyLib.DataObjects.RequestObjects
{
    public class StartTimerRequest
    {
        public TimerType Type { get; set; }
        public Location Location { get; set; }
    }
}