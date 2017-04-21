

namespace Public.Common.Lib.Code
{
    public sealed class MinexRepository : IRepository
    {
        public const string RepositoryName = @"Minex";


        public string Name { get; private set; }


        public MinexRepository()
        {
            this.Name = MinexRepository.RepositoryName;
        }
    }
}
