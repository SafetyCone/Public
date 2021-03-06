﻿using System;


namespace Public.Common.Lib.Organizational
{
    /// <summary>
    /// Specifies information about the organization location of code.
    /// </summary>
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

        public OrganizationalInfo(OrganizationalInfo other)
        {
            this.Organization = other.Organization;
            this.Repository = other.Repository;
            this.Domain = other.Domain;
        }
    }
}
