

namespace Public.Common.Lib.Code
{
    public sealed class ExplorationRepository : IRepository
    {
        public const string RepositoryName = @"Exploration";


        public string Name { get; set; }


        public ExplorationRepository()
        {
            this.Name = ExplorationRepository.RepositoryName;
        }
    }
}
