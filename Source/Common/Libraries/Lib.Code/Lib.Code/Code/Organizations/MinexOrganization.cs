using System;

namespace Public.Common.Lib.Code
{
    public sealed class MinexOrganization : IOrganization
    {
        public const string OrganizationName = @"Minex";


        public string Name { get; private set; }


        public MinexOrganization()
        {
            this.Name = MinexOrganization.OrganizationName;
        }
    }
}
