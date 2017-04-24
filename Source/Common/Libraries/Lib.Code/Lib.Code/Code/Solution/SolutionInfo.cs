using System;

using Public.Common.Lib.Code.Logical;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Holds property values for important details about a project, including solution type and naming info.
    /// </summary>
    /// /// <remarks>
    /// This class is dumb data. The functionality for determining these values lives elsewhere.
    /// </remarks>
    public class SolutionInfo
    {
        public SolutionNamesInfo NamesInfo { get; set; }
        public SolutionType Type { get; set; }


        public SolutionInfo()
        {
            this.NamesInfo = new SolutionNamesInfo();
        }

        public SolutionInfo(SolutionType type, SolutionNamesInfo namesInfo)
        {
            this.Type = type;
            this.NamesInfo = namesInfo;
        }

        public SolutionInfo(SolutionType type)
            : this(type, new SolutionNamesInfo())
        {
        }

        public SolutionInfo(SolutionInfo other)
            : this(other.Type, new SolutionNamesInfo(other.NamesInfo))
        {
        }
    }
}
