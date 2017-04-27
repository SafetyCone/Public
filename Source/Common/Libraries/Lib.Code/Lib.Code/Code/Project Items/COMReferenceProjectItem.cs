using System;


namespace Public.Common.Lib.Code
{
    public class COMReferenceProjectItem : ProjectItem
    {
        public Guid GUID { get; set; }
        public int VersionMajor { get; set; }
        public int VersionMinor { get; set; }
        public int LCID { get; set; }
        public string WrapperTool { get; set; }
        public bool Isolated { get; set; }
        public bool EmbedInteropTypes { get; set; }


        public COMReferenceProjectItem()
        {
        }

        public COMReferenceProjectItem(string includePath)
            : base(includePath)
        {
        }

        public COMReferenceProjectItem(string includePath,
            Guid GUID,
            int versionMajor,
            int versionMinor,
            int LCID,
            string wrapperTool,
            bool isolated,
            bool embedInteropTypes)
            : base(includePath)
        {
            this.GUID = GUID;
            this.VersionMajor = versionMajor;
            this.VersionMinor = versionMinor;
            this.LCID = LCID;
            this.WrapperTool = wrapperTool;
            this.Isolated = isolated;
            this.EmbedInteropTypes = embedInteropTypes;
        }

        public COMReferenceProjectItem(COMReferenceProjectItem other)
            : this(other.IncludePath, other.GUID, other.VersionMajor, other.VersionMinor, other.LCID, other.WrapperTool, other.Isolated, other.EmbedInteropTypes)
        {
        }

        public override ProjectItem Clone()
        {
            return new COMReferenceProjectItem(this);
        }
    }
}
