using System;


namespace Public.Common.Lib.Organizational
{
    /// <summary>
    /// A domain for examples.
    /// </summary>
    /// <remarks>
    /// Use the examples domain to help remember how to use particular repository functionality, or to help remember how to use external functionality used in the repository.
    /// </remarks>
    public sealed class ExamplesDomain
    {
        public const string DomainName = @"Examples";


        public string Name { get; private set; }


        public ExamplesDomain()
        {
            this.Name = ExamplesDomain.DomainName;
        }
    }
}
