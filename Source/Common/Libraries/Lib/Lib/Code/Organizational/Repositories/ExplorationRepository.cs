using System;


namespace Public.Common.Lib.Organizational
{
    /// <summary>
    /// Represents the Exploration repository. The repository is PUBLIC.
    /// </summary>
    /// <remarks>
    /// Aside from the Experiments domain of every repository, and the Experiments application type of every domain, code can be systematically explored in the Experiments repository.
    /// Use this repository for any explorations that should be publically shareable.
    /// </remarks>
    public sealed class ExplorationRepository : IRepository
    {
        public const string RepositoryName = @"Exploration";


        public string Name { get; private set; }
        public bool Public { get; private set; }


        public ExplorationRepository()
        {
            this.Name = ExplorationRepository.RepositoryName;
            this.Public = true;
        }
    }
}
