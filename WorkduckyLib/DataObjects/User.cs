using System.Collections.Generic;
using WorkduckyLib.Interfaces;

namespace WorkduckyLib.DataObjects
{
    public class User : IUser
    {
        public string Uid { get; set; }
        public Settings Settings { get; set; }
        public Company Company { get; set; }
    }
}