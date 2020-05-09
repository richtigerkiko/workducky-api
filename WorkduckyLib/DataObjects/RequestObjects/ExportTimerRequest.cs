using WorkduckyLib.Enums;

namespace WorkduckyLib.DataObjects.RequestObjects
{
    public class ExportTimerRequest
    {
        public int Year { get; set; }
        public FileFormats FileFormat { get; set; }
    }
}
