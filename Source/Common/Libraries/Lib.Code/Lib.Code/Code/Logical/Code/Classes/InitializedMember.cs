using System;


namespace Public.Common.Lib.Code.Logical
{
    public class InitializedMember : Member
    {
        public bool IsConst { get; set; }
        public string Initialization { get; set; }


        public InitializedMember()
            : base()
        {
        }

        public InitializedMember(string name, string typeName, string initialization)
            : base(name, typeName, Accessibility.Private, false)
        {
            this.Initialization = initialization;
        }
    }
}
