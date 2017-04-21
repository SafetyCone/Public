using System;


namespace Public.Common.Lib.Code.Logical
{
    public class MethodArgument
    {
        #region Static

        public static string GetArgumentNameFromTypeName(string typeName)
        {
            string firstChar = typeName.Substring(0, 1);
            string otherChars = typeName.Substring(1);

            string lowerFirstChar = firstChar.ToLowerInvariant();

            string output = lowerFirstChar + otherChars;
            return output;
        }

        #endregion


        public ArgumentPassType PassType { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }


        public MethodArgument()
        {
        }

        public MethodArgument(string typeName, string name, ArgumentPassType passType)
        {
            this.TypeName = typeName;
            this.Name = name;
            this.PassType = passType;
        }

        public MethodArgument(string typeName, string name)
            : this(typeName, name, ArgumentPassType.Normal)
        {
        }

        public MethodArgument(string typeName)
            : this(typeName, MethodArgument.GetArgumentNameFromTypeName(typeName))
        {
        }
    }
}
