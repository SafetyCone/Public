using System;
using System.Collections.Generic;


namespace Public.Common.Lib.Code.Logical
{
    /// <summary>
    /// Represents a property, which is a get method, a set method, and a data member all in one.
    /// </summary>
    public class PropertyMember : Member
    {
        public Accessibility GetAccessibility { get; set; }
        public List<string> GetMethodLines { get; protected set; }
        public Accessibility SetAccessibility { get; set; }
        public List<string> SetMethodLines { get; protected set; }


        public PropertyMember()
            : base()
        {
            this.Setup();
        }

        private void Setup()
        {
            this.GetMethodLines = new List<string>();
            this.SetMethodLines = new List<string>();

            this.GetAccessibility = this.Accessibility;
            this.SetAccessibility = this.Accessibility;
        }

        public PropertyMember(string name, string typeName)
            :base(name, typeName)
        {
            this.Setup();
        }
    }
}
