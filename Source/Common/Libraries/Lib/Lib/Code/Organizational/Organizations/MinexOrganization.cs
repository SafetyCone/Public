using System;


namespace Public.Common.Lib.Organizational
{
    /// <summary>
    /// Represents the Minex organization.
    /// </summary>
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
