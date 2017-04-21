

namespace Public.Common.Lib.Code
{
    public sealed class CommonDomain : IDomain
    {
        public const string DomainName = @"Common";


        public string Name { get; private set; }


        public CommonDomain()
        {
            this.Name = CommonDomain.DomainName;
        }
    }
}
