using System;


namespace ExaminingClasses.Lib
{
    /// <summary>
    /// A class with a default constructor.
    /// </summary>
    public class DefaultConstructorClass
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }


        public DefaultConstructorClass()
        {
            this.Value1 = 1;
            this.Value2 = 2;
        }
    }
}
