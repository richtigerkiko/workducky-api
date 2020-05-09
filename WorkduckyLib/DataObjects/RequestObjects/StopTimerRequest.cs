using System.ComponentModel.DataAnnotations;
using WorkduckyLib.Interfaces;

namespace WorkduckyLib.DataObjects.RequestObjects
{
    public class StopTimerRequest
    {
        public Location Location { get; set; }
        public string TimerId { get; set; }
    }
}