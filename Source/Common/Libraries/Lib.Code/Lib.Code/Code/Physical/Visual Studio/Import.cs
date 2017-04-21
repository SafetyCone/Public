using System;


namespace Public.Common.Lib.Code.Physical
{
    /// <summary>
    /// Represents an import used in the build of a project file.
    /// </summary>
    public class Import : IEquatable<Import>
    {
        #region Static

        public static readonly Import MicrosoftCommonProps = new Import(
            @"$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props",
            @"Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')");
        public static readonly Import MicrosoftCSharpTargets = new Import(
            @"$(MSBuildToolsPath)\Microsoft.CSharp.targets");


        public static bool operator ==(Import lhs, Import rhs)
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

        public static bool operator !=(Import lhs, Import rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        #region IEquatable<EqualsClass> Members

        public bool Equals(Import other)
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
            output = output && (this.ProjectPath == other.ProjectPath);
            output = output && (this.Condition == other.Condition);

            return output;
#else
            return
                (this.ProjectPath == other.ProjectPath) &&
                (this.Condition == other.Condition);
#endif
        }

        #endregion


        public string ProjectPath { get; set; }
        public string Condition { get; set; }


        public Import()
        {
        }

        public Import(string projectPath)
            : this(projectPath, null)
        {
        }

        public Import(string projectPath, string condition)
        {
            this.ProjectPath = projectPath;
            this.Condition = condition;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Import);
        }

        public override int GetHashCode()
        {
            return HashHelper.GetHashCode(this.ProjectPath, this.Condition);
        }
    }
}
