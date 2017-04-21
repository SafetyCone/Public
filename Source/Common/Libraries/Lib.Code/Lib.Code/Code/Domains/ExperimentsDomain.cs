

namespace Public.Common.Lib.Code
{
    public sealed class ExperimentsDomain
    {
        public const string DomainName = @"Experiments";


        public string Name { get; private set; }


        public ExperimentsDomain()
        {
            this.Name = ExperimentsDomain.DomainName;
        }
    }
}
