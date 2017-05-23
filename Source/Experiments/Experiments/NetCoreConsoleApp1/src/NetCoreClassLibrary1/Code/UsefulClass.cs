using System;


namespace NetCoreClassLibrary1
{
    public class UsefulClass
    {
        public const string DefaultValue = @"Hello World!";


        public string Value { get; set; }


        public UsefulClass(string value)
        {
            this.Value = value;
        }

        public UsefulClass()
            : this(UsefulClass.DefaultValue)
        {
        }
    }
}
