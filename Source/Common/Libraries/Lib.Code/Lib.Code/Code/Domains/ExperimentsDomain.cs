

namespace Public.Common.Lib.Code
{
    /// <summary>
    /// A domain for proof-of-concept, or scratch-work, or other experimental code.
    /// </summary>
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
