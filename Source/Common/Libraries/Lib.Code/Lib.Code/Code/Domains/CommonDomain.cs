

namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Represents THE domain of a repository, in which everything common to all domains of a repository should be placed.
    /// </summary>
    /// <remarks>
    /// The common domain primarily contains libraries, but any applications, scripts, experiments, or web sites of common use throughout a repository should go in the common domain.
    /// </remarks>
    public sealed class CommonDomain : IDomain
    {
        public const string DomainName = @"Common";


        public string Name { get; private set; }


        public CommonDomain()
        {
            this.Name = CommonDomain.DomainName;
        }
    }
}
