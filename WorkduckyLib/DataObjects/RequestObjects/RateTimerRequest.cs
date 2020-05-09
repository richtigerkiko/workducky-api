using System.ComponentModel.DataAnnotations;
using WorkduckyLib.Interfaces;

namespace WorkduckyLib.DataObjects.RequestObjects
{
    public class RateTimerRequest
    {
        public string TimerId { get; set; }
        public string Notes { get; set; }

        [Range(0, 10, ErrorMessage = "Rating must be between 0 and 10")]
        public int Score { get; set; }
    }
}