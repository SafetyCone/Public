using System;

using Public.Common.Lib.Code.Physical;
using Public.Common.Lib.IO.Serialization;


namespace Public.Common.Lib.Code.Serialization
{
    /// <summary>
    /// A Visual Studio solution file serialization unit.
    /// </summary>
    public class SolutionSerializationUnit : SerializationUnitBase
    {
        public Solution Solution { get; set; }


        public SolutionSerializationUnit(string path, Solution solution)
            : base(path)
        {
            this.Solution = solution;
        }
    }
}
