using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Represents the Minex repository. This repository is PRIVATE.
    /// </summary>
    public sealed class MinexRepository : IRepository
    {
        public const string RepositoryName = @"Minex";


        public string Name { get; private set; }
        public Accessibility Accessibility { get; private set; }


        public MinexRepository()
        {
            this.Name = MinexRepository.RepositoryName;
            this.Accessibility = Accessibility.Private;
        }
    }
}
