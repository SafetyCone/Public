using System;


namespace ExaminingClasses.Lib
{
    /// <summary>
    /// Second-level class in an inheritance hierarchy.
    /// </summary>
    public class ClassB : ClassA, IInterfaceB
    {
        public double ValueTypeValueB { get; set; }
        public string ReferenceTypeValueB { get; set; }


        public override void MethodA()
        {
            Console.WriteLine($@"{nameof(ClassB)}:{nameof(ClassB.MethodA)}");
        }

        public void MethodB()
        {
            Console.WriteLine($@"{nameof(ClassB)}:{nameof(ClassB.MethodB)}");
        }

        protected override void ProtectedMethod()
        {
            Console.WriteLine($@"{nameof(ClassB)}:{nameof(ClassB.ProtectedMethod)}");
        }
    }
}
