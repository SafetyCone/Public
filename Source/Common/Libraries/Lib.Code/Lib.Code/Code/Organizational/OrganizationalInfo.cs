using System;


namespace Public.Common.Lib.Code
{
    // Ok.
    public class OrganizationalInfo
    {
        public string Organization { get; set; }
        public string Repository { get; set; }
        public string Domain { get; set; }


        public OrganizationalInfo()
        {
        }

        public OrganizationalInfo(string organization, string repository, string domain)
        {
            this.Organization = organization;
            this.Repository = repository;
            this.Domain = domain;
        }
    }
}
