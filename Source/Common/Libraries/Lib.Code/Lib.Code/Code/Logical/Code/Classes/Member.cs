

namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Represents a data member.
    /// </summary>
    public class Member : LogicalObjectBase
    {
        public bool IsStatic { get; set; } // Static cannot be abstract, virtual, or override; or sealed.
        public bool IsAbstract { get; set; } // Abstract cannot be virtual or override.
        public bool IsVirtual { get; set; } // Virtual cannot be override.
        public bool IsOverride { get; set; } // Override can only be override.
        public bool IsNew { get; set; } // New can be anything.
        public string ReturnTypeName { get; set; }


        public Member()
        {
        }

        public Member(string name, string typeName, Accessibility accessibility, bool isStatic)
        {
            this.Accessibility = Accessibility;
            this.IsStatic = isStatic;
            this.ReturnTypeName = typeName;
            this.Name = name;
        }

        public Member(string name, string typeName)
            : this(name, typeName, Accessibility.Private, false)
        {
        }
    }
}
