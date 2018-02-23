using System;

using Public.Common.Lib.Organizational;


namespace Public.Malachite.Lib.Organizational
{
    public class MalachiteDomain : IDomain
    {
        public static readonly string DomainName = @"Malachite";
        public static readonly string DomainDirectoryName = MalachiteDomain.DomainName;


        public string Name { get; private set; }


        public MalachiteDomain()
        {
            this.Name = MalachiteDomain.DomainName;
        }
    }
}
