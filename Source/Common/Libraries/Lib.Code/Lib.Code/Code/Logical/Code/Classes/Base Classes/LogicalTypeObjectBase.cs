using System;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Base class for all logical types that can be their own type (classes, structs, delegates, enums; NOT members and methods).
    /// </summary>
    public abstract class LogicalTypeObjectBase : LogicalObjectBase
    {
        #region Static

        public static string GetTypeKey(LogicalTypeObjectBase typeObject)
        {
            string output = LogicalTypeObjectBase.GetTypeKey(typeObject.NamespaceName, typeObject.Name);
            return output;
        }

        public static string GetTypeKey(string namespaceName, string className)
        {
            string output = namespaceName + Constants.NamespaceTokenSeparator + className;
            return output;
        }

        #endregion


        public string NamespaceName { get; set; }
        public virtual string TypeKey
        {
            get
            {
                return LogicalTypeObjectBase.GetTypeKey(this);
            }
        }


        protected LogicalTypeObjectBase()
             : base()
        {
        }

        protected LogicalTypeObjectBase(string name, string namespaceName)
            : base(name, Accessibility.Public)
        {
            this.NamespaceName = namespaceName;
        }
    }
}
