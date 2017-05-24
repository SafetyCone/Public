using System;


namespace Public.Common.Lib.Organizational
{
    /// <summary>
    /// Represents the Minex repository. This repository is PRIVATE.
    /// </summary>
    public sealed class MinexRepository : IRepository
    {
        public const string RepositoryName = @"Minex";


        public string Name { get; private set; }
        public bool Public { get; private set; }


        public MinexRepository()
        {
            this.Name = MinexRepository.RepositoryName;
            this.Public = false;
        }
    }
}
