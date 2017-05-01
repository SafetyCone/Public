using System;
using System.IO;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// Used in determining which projects need to be distributed between solutions.
    /// </summary>
    /// <remarks>
    /// For a Visual Studio versioned project file name, we do not want to use the GUID to determine the identity of a project.
    /// Instead, we want to use the base-file named relative path.
    /// 
    /// For a project file name without a Visual Studio version token, we want to use the GUID.
    /// </remarks>
    public class ProjectReferenceDiffItem : IEquatable<ProjectReferenceDiffItem>
    {
        #region Static

        public static bool operator ==(ProjectReferenceDiffItem lhs, ProjectReferenceDiffItem rhs)
        {
            if (object.ReferenceEquals(null, lhs))
            {
                if (object.ReferenceEquals(null, rhs))
                {
                    return true;
                }

                return false;
            }

            return lhs.Equals(rhs); // Equals() handles a null right side.
        }

        public static bool operator !=(ProjectReferenceDiffItem lhs, ProjectReferenceDiffItem rhs)
        {
            return !(lhs == rhs);
        }

        private static int GetInstanceHashValue(ProjectReferenceDiffItem item)
        {
            int output;
            if(VisualStudioVersionSet.IsVisualStudioVersioned(item.RelativePath))
            {
                string fileNameBase = ProjectFileNameInfo.GetFileNameBase(item.RelativePath);

                string directoryName = Path.GetDirectoryName(item.RelativePath);
                string baseFileNamePath = Path.Combine(directoryName, fileNameBase);

                output = baseFileNamePath.GetHashCode();
            }
            else
            {
                output = item.GUID.GetHashCode();
            }

            return output;
        }

        #endregion

        #region IEquatable<ProjectReferenceDiffItem> Members

        public bool Equals(ProjectReferenceDiffItem other)
        {
            if (object.ReferenceEquals(null, other))
            {
                return false;
            }

            if (object.ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.GetType() != other.GetType())
            {
                return false;
            }

#if DEBUG
            bool output = true;
            output = output && (this.HashValue == other.HashValue);

            return output;
#else
            return
                (this.HashValue == other.HashValue);
#endif
        }

        #endregion


        public string RelativePath { get; set; }
        public Guid GUID { get; set; }
        public int HashValue { get; set; }


        public ProjectReferenceDiffItem(string relativePath, Guid guid)
        {
            this.RelativePath = relativePath;
            this.GUID = guid;

            this.HashValue = ProjectReferenceDiffItem.GetInstanceHashValue(this);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ProjectReferenceDiffItem);
        }

        public override int GetHashCode()
        {
            return this.HashValue;
        }
    }
}
