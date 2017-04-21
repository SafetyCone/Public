using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Lists all domains.
    /// </summary>
    public static class Domains
    {
        public static readonly CommonDomain Common = new CommonDomain();
        public static readonly ExamplesDomain Examples = new ExamplesDomain();
        public static readonly ExperimentsDomain Experiments = new ExperimentsDomain();
    }
}
