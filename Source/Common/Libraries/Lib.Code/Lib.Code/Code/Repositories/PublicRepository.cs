

namespace Public.Common.Lib.Code
{
    public sealed class PublicRepository : IRepository
    {
        public const string RepositoryName = @"Public";


        public string Name { get; private set; }
        public CommonDomain Common { get; private set; }
        public ExamplesDomain Examples { get; private set; }
        public ExperimentsDomain Experiments { get; private set; }


        public PublicRepository()
        {
            this.Name = PublicRepository.RepositoryName;
            this.Common = new CommonDomain();
            this.Examples = new ExamplesDomain();
        }
    }
}
