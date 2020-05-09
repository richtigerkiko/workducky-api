using System;
using System.Collections.Generic;

namespace WorkduckyLib.DataObjects.dbo.Mongo
{
    public class CompanyDocument
    {
        public string _id { get; set; }
        public string CompanyName { get; set; }
        public Address Address { get; set; }
        public List<Location> Locations { get; set; }

        public CompanyDocument()
        {
            _id = MongoUtility.GenerateId();
        }

        public static implicit operator CompanyDocument(Company company)
        {
            var companyDocument = new CompanyDocument()
            {
                _id = company.Cid,
                Address = company.Address,
                CompanyName = company.CompanyName,
                Locations = company.Locations
            };
            return companyDocument;
        }

        public static implicit operator Company(CompanyDocument companyDocument)
        {
            var company = new Company()
            {
                Cid = companyDocument._id,
                Address = companyDocument.Address,
                CompanyName = companyDocument.CompanyName,
                Locations = companyDocument.Locations
            };
            return company;
        }
    }
}