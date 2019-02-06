using System;


namespace ExaminingClasses.Lib
{
    /// <summary>
    /// Non-abstract base-class for an example class hierarchy.
    /// </summary>
    public class ClassA : IInterfaceA
    {
        public int ValueTypeValueA { get; set; } // Value-type (Int32) property.
        public string ReferenceTypeValueA { get; set; } // Reference-type (String, even though it acts like a value-type) property.


        public virtual void MethodA()
        {
            Console.WriteLine($@"{nameof(ClassA)}:{nameof(ClassA.MethodA)}");
        }

        protected virtual void ProtectedMethod()
        {
            Console.WriteLine($@"{nameof(ClassA)}:{nameof(ClassA.ProtectedMethod)}");
        }
    }
}
