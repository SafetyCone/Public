

namespace Public.Common.Lib.Code
{
    public sealed class ExamplesDomain
    {
        public const string DomainName = @"Examples";


        public string Name { get; private set; }


        public ExamplesDomain()
        {
            this.Name = ExamplesDomain.DomainName;
        }
    }
}
