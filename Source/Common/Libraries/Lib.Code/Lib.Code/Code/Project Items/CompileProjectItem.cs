using System;


namespace Public.Common.Lib.Code
{
    /// <summary>
    /// The relative path of compiled file in a project.
    /// </summary>
    public class CompileProjectItem : ProjectItem
    {
        public bool AutoGen { get; set; }
        public string DependentUpon { get; set; }
        public bool DesignTime { get; set; }
        public bool DesignTimeSharedInput { get; set; }


        public CompileProjectItem()
        {
        }

        public CompileProjectItem(string includePath)
            : base(includePath)
        {
        }

        public CompileProjectItem(string includePath, bool autoGen, string dependentUpon, bool designTime, bool designTimeSharedInput)
            : base(includePath)
        {
            this.AutoGen = autoGen;
            this.DependentUpon = dependentUpon;
            this.DesignTime = designTime;
            this.DesignTimeSharedInput = designTimeSharedInput;
        }

        public CompileProjectItem(CompileProjectItem other)
            :this(other.IncludePath, other.AutoGen, other.DependentUpon, other.DesignTime, other.DesignTimeSharedInput)
        {
        }

        public override ProjectItem Clone()
        {
            return new CompileProjectItem(this);
        }
    }
}
