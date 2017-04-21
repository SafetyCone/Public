using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    public class Method : LogicalObjectBase
    {
        #region Static

        /// <summary>
        /// A constructor.
        /// </summary>
        public static Method NewStaticMethod(string name, string returnTypeName, Accessibility accessibility, IEnumerable<MethodArgument> arguments)
        {
            return new Method(name, returnTypeName, accessibility, arguments, true);
        }

        #endregion


        public bool IsStatic { get; set; } // Static cannot be abstract, virtual, or override; or sealed.
        public bool IsAbstract { get; set; } // Abstract cannot be virtual or override.
        public bool IsVirtual { get; set; } // Virtual cannot be override.
        public bool IsOverride { get; set; } // Override can only be override.
        public bool IsNew { get; set; } // New can be anything.
        public string ReturnTypeName { get; set; }
        public List<MethodArgument> Arguments { get; protected set; }
        public List<string> Lines { get; protected set; }


        public Method()
        {
            this.Arguments = new List<MethodArgument>();
            this.Lines = new List<string>();
        }

        public Method(string name, string returnTypeName, Accessibility accessibility, IEnumerable<MethodArgument> arguments, bool isStatic)
            : this()
        {
            this.Accessibility = Accessibility;
            this.IsStatic = isStatic;
            this.ReturnTypeName = returnTypeName;
            this.Name = name;

            if (null != arguments)
            {
                this.Arguments.AddRange(arguments);
            }
        }

        public Method(string name, string returnTypeName)
            : this(name, returnTypeName, Accessibility.Private, null, false)
        {
        }

        public Method(string name)
            : this(name, Types.VoidTypeName)
        {
        }
    }
}
