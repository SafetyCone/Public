using System;


namespace Public.Common.Lib.Organizational
{
    /// <summary>
    /// Represents the public repository. This repository is PUBLIC.
    /// </summary>
    /// <remarks>
    /// The public domain should contain all non-proprietary code of general use.
    /// </remarks>
    public sealed class PublicRepository : IRepository
    {
        public static readonly string RepositoryName = @"Public";
        public static readonly string RepositoryDirectoryName = PublicRepository.RepositoryName;


        public string Name { get; private set; }
        public bool Public { get; private set; }
        public CommonDomain Common { get; private set; }
        public ExamplesDomain Examples { get; private set; }
        public ExperimentsDomain Experiments { get; private set; }


        public PublicRepository()
        {
            this.Name = PublicRepository.RepositoryName;
            this.Public = true;
            this.Common = new CommonDomain();
            this.Examples = new ExamplesDomain();
            this.Experiments = new ExperimentsDomain();
        }
    }
}
