using System.Collections.Generic;

namespace WorkduckyLib.DataObjects
{
    public class Company
    {
        public string Cid { get; set; }
        public string CompanyName { get; set; }
        public Address Address { get; set; }
        public List<Location> Locations { get; set; }
    }
}