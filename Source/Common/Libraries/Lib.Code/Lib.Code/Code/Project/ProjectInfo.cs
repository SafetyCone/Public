using System;

using Public.Common.Lib.Code.Logical;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Holds property values for important details about a project, including GUID, naming info, language, and project type.
    /// </summary>
    /// <remarks>
    /// This class is dumb data. The functionality for determining these values lives elsewhere.
    /// </remarks>
    public class ProjectInfo
    {
        public ProjectNamesInfo NamesInfo { get; set; }
        public Guid GUID { get; set; }
        public Language Language { get; set; }
        public ProjectType Type { get; set; }


        public ProjectInfo()
        {
            this.NamesInfo = new ProjectNamesInfo();
        }

        public ProjectInfo(Guid guid, Language language, ProjectType type, ProjectNamesInfo namesInfo)
        {
            this.GUID = guid;
            this.NamesInfo = namesInfo;
            this.Language = language;
            this.Type = type;
        }

        public ProjectInfo(Guid guid, Language language, ProjectType type)
            : this(guid, language, type, new ProjectNamesInfo())
        {
        }

        public ProjectInfo(ProjectInfo other)
            : this(other.GUID, other.Language, other.Type, new ProjectNamesInfo(other.NamesInfo))
        {
        }
    }
}
