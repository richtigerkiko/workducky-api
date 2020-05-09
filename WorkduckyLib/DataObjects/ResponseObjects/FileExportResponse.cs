using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WorkduckyLib.DataObjects.ResponseObjects
{
    public class FileExportResponse
    {
        public Stream FileStream{ get; set; }
        public string MimeType { get; set; }
    }
}
